using Microsoft.AspNetCore.Diagnostics;
using QrAttendanceApi.Application.Responses;
using System.Net;
using System.Text.Json;

namespace QrAttendanceApi.Core.Extensions
{
    public static class AppExtensions
    {
        internal static void UseAppMiddlewares(this WebApplication app, ILogger<Program> logger)
        {
            app.UseGlobalExceptionHandler(logger);
            app.UseSwaggerDocsUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
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
