using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Responses;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Domain.Enums;
using System.Net;
using System.Text.Json;

namespace QrAttendanceApi.Core.Extensions
{
    public static class AppExtensions
    {
        internal static async Task UseAppMiddlewares(this WebApplication app, ILogger<Program> logger)
        {
            app.UseGlobalExceptionHandler(logger);
            if (!app.Environment.IsDevelopment())
            {
                await app.SeedAsync(logger);
            }
            app.UseSwaggerDocsUI();
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }

        private static async Task SeedAsync(this WebApplication app, ILogger<Program> logger)
        {
            using var scope = app.Services.CreateScope();
            await SeedAdminUser(scope, logger);
        }

        private static async Task SeedAdminUser(IServiceScope scope, ILogger<Program> logger)
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var repository = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();


            if (userManager != null && config != null)
            {
                var email = config["System:Email"];
                var password = config["System:Password"];
                var phone = config["System:PhoneNumber"];
                if(!string.IsNullOrWhiteSpace(email) && 
                    !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(phone))
                {
                    logger.LogInformation("Looking for existing user...");
                    var user = await userManager.FindByEmailAsync(email);
                    if (user == null)
                    {
                        var department = await repository.Department.Get(d => !d.IsDeprecated)
                            .FirstOrDefaultAsync();
                        if(department == null)
                        {
                            department = new Department
                            {
                                Name = ".NET"
                            };
                            await repository.Department.AddAsync(department);
                            await repository.SaveAsync();
                        }
                        logger.LogInformation("Existing system user not found. Inserting...");
                        user = new User
                        {
                            FullName = "System User",
                            Email = email,
                            EmailConfirmed = true,
                            UserName = email,
                            PhoneNumber = phone,
                            IsActive = true,
                            ExternalId = "CNGNET08",
                            DepartmentId = department.Id,
                        };

                        var createResult = await userManager.CreateAsync(user, password);
                        if (createResult.Succeeded)
                        {
                            logger.LogInformation("Successfully created system user...");
                            var roleResult = await userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
                            if (!roleResult.Succeeded)
                            {
                                await userManager.DeleteAsync(user);
                            }
                            logger.LogInformation("Successfully assigned role to the system user...");
                            logger.LogInformation("System user successfully added");
                        }
                        return;
                    }
                }
            }
        }

        private static WebApplication UseSwaggerDocsUI(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                opt.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
            });

            return app;
        }

        private static WebApplication UseGlobalExceptionHandler(this WebApplication app, ILogger<Program> logger)
        {
            app.UseExceptionHandler(b =>
            {
                b.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if(feature != null)
                    {
                        logger.LogError("An error occurred: {Error}", feature.Error);

                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var response = JsonSerializer.Serialize(new ErrorResponse
                        {
                            Status = context.Response.StatusCode,
                            Success = false,
                            Message = feature.Error.Message,
                        });

                        await context.Response.WriteAsync(response);
                    }
                });
            });

            return app;
        }
    }
}
