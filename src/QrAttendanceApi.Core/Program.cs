using Hangfire;
using QrAttendanceApi.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterServices(builder.Configuration);
builder.Host.ConfigireSerilogEsSink(builder.Configuration);
var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
await app.UseAppMiddlewares(logger);
app.UseHangfireDashboard();
app.Run();