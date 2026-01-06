using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Commands.Accounts;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Helpers;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Application.Settings;
using QrAttendanceApi.Application.Validations;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;

namespace QrAttendanceApi.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly List<string> allowedFileType = new List<string> { ".xls", ".xlsx" };
        private const long maxDocSize = 5120000;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepositoryManager _repository;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly JwtSettings _settings;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IOptions<JwtSettings> options,
                              IRepositoryManager repository,
                              ITokenService tokenService,
                              IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
            _tokenService = tokenService;
            _emailService = emailService;
            _settings = options.Value;
        }

        public async Task<ApiBaseResponse> RegisterAsync(RegisterCommand command)
        {
            var validator = new RegisterCommandValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid input");
            }

            var existingUser = await _userManager.FindByEmailAsync(command.Email);
            if (existingUser != null)
            {
                return new ConflictResponse("User with this email already exists. Please login instead");
            }

            var user = RegisterCommand.MapUser(command);
            user.FullName = StringHelpers.ConvertEachWordToUppercase(user.FullName);
            var createdResult = await _userManager.CreateAsync(user, command.Password);
            if (!createdResult.Succeeded)
            {
                return new BadRequestResponse(createdResult.Errors.FirstOrDefault()?.Description ?? "Registration failed");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, command.Role.ToString());
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return new BadRequestResponse(roleResult.Errors.FirstOrDefault()?.Description ?? "Registration failed.");
            }

            return new OkResponse<string>("Registration successful. Please verify your account");
        }

        public async Task<ApiBaseResponse> LoginAsync(LoginCommand command)
        {
            var validator = new LoginCommandValidator().Validate(command);
            if (!validator.IsValid)
            {
                return new BadRequestResponse(validator.Errors.FirstOrDefault()?.ErrorMessage ?? "Invalid inputs");
            }

            var user = await _userManager.FindByEmailAsync(command.Email);
            if(user == null)
            {
                return new NotFoundResponse("No user record found for this email address");
            }

            if(!user.IsActive || !user.EmailConfirmed)
            {
                return new ForbiddenResponse("You are not allowed to perform this action. Please contact support.");
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, command.Password, lockoutOnFailure: true);
            if (!passwordCheck.Succeeded)
            {
                return new BadRequestResponse("Wrong email and password combination");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles == null || roles.Count <= 0)
            {
                return new ForbiddenResponse("You can not login at this time");
            }

            var accessToken = _tokenService.CreateAccessToken(user, roles.ToArray());
            var refreshToken = await _tokenService.CreateAndSaveRefreshToken(user.Id);

            return new OkResponse<LoginTokensDto>(new LoginTokensDto(accessToken, refreshToken));
        }

        public async Task<ApiBaseResponse> RefreshAsync(RefreshTokenCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.RefreshToken))
            {
                return new UnauthorizedResponse("Access denied. Please login");
            }

            var hash = _tokenService.ComputeHash(command.RefreshToken);
            var tokenFromDb = await _repository.Token
                .FirstOrDefault(t => !t.IsDeprecated && t.Hash == hash, true);
            if (tokenFromDb == null)
            {
                return new UnauthorizedResponse("Access denied. Please login");
            }

            if(tokenFromDb.ExpiresAt < DateTimeOffset.UtcNow)
            {
                tokenFromDb.IsDeprecated = true;
                _repository.Token.Update(tokenFromDb);
                await _repository.SaveAsync();

                return new UnauthorizedResponse("Access denied. Please login");
            }

            var user = await _userManager.FindByIdAsync(tokenFromDb.UserId);
            if(user == null)
            {
                return new NotFoundResponse("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = _tokenService.CreateAccessToken(user, roles.ToArray());
            return new OkResponse<LoginTokensDto>(new LoginTokensDto(newAccessToken, command.RefreshToken));
        }

        public async Task<ApiBaseResponse> LoadUserDataAsync(IFormFile file)
        {
            var validationResult = ValidateFile(file);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "upload");
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var filePath = Path.Combine(uploadDirectory, Guid.NewGuid() + fileExtension);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var jobId = BackgroundJob.Enqueue(() => LoadUsers(filePath, null!));
            return new OkResponse<string>($"User Bulk upload started. You can monitor the progress from Hangfire dashboard. Job Id: {jobId}");
        }

        public async Task LoadUsers(string filePath, PerformContext context)
        {
            context.WriteLine("[LoadUsers]:  Running user bulk uploads");
            if (!File.Exists(filePath))
            {
                context.WriteLine("[LoadUsers]: File {filePath} not found.");
                return;
            }

            var bytes = File.ReadAllBytes(filePath);
            context.WriteLine($"[LoadUsers]:  Successfully read the file content from path: {filePath}");

            using var stream = new MemoryStream(bytes);
            if (stream.Length <= 0)
            {
                context.WriteLine("[LoadUsers]:  Invalid stream length");
                return;
            }

            context.WriteLine("[LoadUsers]:  Loading data from the file...");
            using var workbook = new XLWorkbook(stream);
            var sheet = workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
            {
                context.WriteLine("[LoadUsers]:  No excel sheet found for the file.");
                return;
            }

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                context.WriteLine("[LoadUsers]:  File deleted after loading the data.");
            }

            var departments = await _repository.Department
                .Get(d => !d.IsDeprecated).ToListAsync();
            foreach (var row in sheet.RowsUsed().Skip(1))
            {
                var roleString = row.Cell(6).GetString();
                if (!Enum.TryParse<Roles>(roleString, true, out var @role) || !Enum.IsDefined(typeof(Roles), @role))
                {
                    @role = Roles.Student;
                }

                var dept = row.Cell(7).GetString();
                var department = departments
                    .Where(d => d.Name.ToLower().Equals(dept.ToLower())).FirstOrDefault();

                if (department != null)
                {
                    var payload = new RegisterCommand
                    {
                        FullName = row.Cell(1).GetString(),
                        Email = row.Cell(2).GetString(),
                        PhoneNumber = string.Concat("+", row.Cell(3).GetString()),
                        Password = row.Cell(4).GetString(),
                        ComfirmPassword = row.Cell(5).GetString(),
                        Role = @role,
                        DepartmentId = department.Id
                    };

                    var validator = new RegisterCommandValidator().Validate(payload);
                    if (validator.IsValid)
                    {
                        var response = await RegisterAsync(payload);
                        if (response.Success)
                        {
                            //TODO: Send email to the users here informing them of their addition to the system.
                            try
                            {
                            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "user-addition-email.html");
                            var template = await File.ReadAllTextAsync(templatePath);
                            var emailBody = template
                                            .Replace("{{FullName}}", payload.FullName)
                                            .Replace("{{Email}}", payload.Email)
                                            .Replace("{{Password}}", payload.Password);

                            await _emailService.SendAsync(
                                payload.Email,
                                emailBody,
                                "Welcome to QR Attendance System"
                            );
                            //You're expected to send them an email. User the user-addition-email.html template in the wwwroot
                            context.WriteLine($"User, {payload.FullName} successfully registered!");
                            }
                            catch (Exception ex)
                            {
                                context.WriteLine($"User registered but email failed: {ex.Message}");
                            }
                        }
                        else
                        {
                            context.WriteLine($"Registration failed for {payload.FullName}. Reason: {response.Message}");
                        }
                    }
                    else
                    {
                        context.WriteLine($"[LoadUsers]: Invalid input for {payload.Email}!. Validation messages {string.Join(",", validator.Errors.Select(e => e.ErrorMessage))}");
                    }
                }
            }
        }

        #region Private Methods
        private ApiBaseResponse ValidateFile(IFormFile file)
        {
            if (file is null || file.Length <= 0)
            {
                return new BadRequestResponse("Please upload a file.");
            }

            if (!allowedFileType.Any(f => file.FileName.EndsWith(f)))
            {
                return new BadRequestResponse("Please selected a valid file type");
            }

            if (file.Length > maxDocSize)
            {
                return new BadRequestResponse("File too large. Maximum size is 5mb");
            }

            return new OkResponse<string>("File is valid");
        }
        #endregion
    }
}
