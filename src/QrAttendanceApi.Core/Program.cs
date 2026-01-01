//using Hangfire;
//using QrAttendanceApi.Core.Extensions;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.RegisterServices(builder.Configuration);
//builder.Host.ConfigireSerilogEsSink(builder.Configuration);
//var app = builder.Build();

//var logger = app.Services.GetRequiredService<ILogger<Program>>();
//await app.UseAppMiddlewares(logger);
//app.UseHangfireDashboard();
//app.Run();
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAttendanceApi.Core.Extensions;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ------------------------
// Database & Identity
// ------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

//builder.Services.AddIdentity<User, Microsoft.AspNetCore.Identity.IdentityRole>()
//    .AddEntityFrameworkStores<AppDbContext>()
//    .AddDefaultTokenProviders();

// ------------------------
// Hangfire (PostgreSQL storage)
// ------------------------
builder.Services.AddHangfire(configuration =>
{
    configuration.UsePostgreSqlStorage(
        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});
builder.Services.AddHangfireServer();

// ------------------------
// Custom services & logging
// ------------------------
builder.Services.RegisterServices(builder.Configuration);
builder.Host.ConfigireSerilogEsSink(builder.Configuration);

var app = builder.Build();

// ------------------------
// Middlewares
// ------------------------
var logger = app.Services.GetRequiredService<ILogger<Program>>();
await app.UseAppMiddlewares(logger);

app.UseHangfireDashboard(); // Dashboard at /hangfire
app.Run();
