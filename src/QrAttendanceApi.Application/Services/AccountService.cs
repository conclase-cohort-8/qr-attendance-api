using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Commands.Accounts;
using QrAttendanceApi.Application.DTOs;
using QrAttendanceApi.Application.Helpers;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Application.Settings;
using QrAttendanceApi.Application.Validations;
using QrAttendanceApi.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QrAttendanceApi.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IRepositoryManager _repository;
        private readonly JwtSettings _settings;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IOptions<JwtSettings> options,
                              IRepositoryManager repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
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

            var accessToken = CreateAccessToken(user, roles.ToArray());
            var refreshToken = await CreateAndSaveRefreshToken(user.Id);

            return new OkResponse<TokenDto>(new TokenDto(accessToken, refreshToken));
        }

        private string CreateAccessToken(User user, string[] roles)
        {
            var claims = GetClaims(user, roles);
            var creds = GetSigningCredentials();
            var token = GetSecurityToken(claims, creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ApiBaseResponse> RefreshAsync(RefreshTokenCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.RefreshToken))
            {
                return new UnauthorizedResponse("Access denied. Please login");
            }

            var hash = ComputeHash(command.RefreshToken);
            var tokenFromDb = await _repository.Token
                .FindAsync(t => !t.IsDeprecated && t.Hash == hash);
            if (tokenFromDb == null)
            {
                return new UnauthorizedResponse("Access denied. Please login");
            }

            if(tokenFromDb.ExpiresAt < DateTimeOffset.UtcNow)
            {
                tokenFromDb.IsDeprecated = true;
                await _repository.Token.UpdateAsync(tokenFromDb);

                return new UnauthorizedResponse("Access denied. Please login");
            }

            var user = await _userManager.FindByIdAsync(tokenFromDb.UserId);
            if(user == null)
            {
                return new NotFoundResponse("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var newAccessToken = CreateAccessToken(user, roles.ToArray());
            return new OkResponse<TokenDto>(new TokenDto(newAccessToken, command.RefreshToken));
        }

        #region Private Methods
        private List<Claim> GetClaims(User user, string[] roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_settings.Key);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GetSecurityToken(List<Claim> claims, SigningCredentials credentials)
        {
            return new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(_settings.Expires),
                signingCredentials: credentials
            );
        }

        private async Task<string> CreateAndSaveRefreshToken(string userId)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var hash = ComputeHash(token);

            await _repository.Token.AddAsync(new RefreshToken
            {
                Hash = hash,
                UserId = userId,
                ExpiresAt = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(_settings.Expires))
            });

            return token;
        }

        private string ComputeHash(string token)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }

        #endregion
    }
}
