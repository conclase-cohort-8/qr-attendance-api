using Hangfire;
using Hangfire.Console;
using Hangfire.PostgreSql;
using Mailjet.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QrAttendanceApi.Application.Abstractions;
using QrAttendanceApi.Application.Abstractions.Externals;
using QrAttendanceApi.Application.Services;
using QrAttendanceApi.Application.Services.Abstractions;
using QrAttendanceApi.Application.Settings;
using QrAttendanceApi.Domain.Entities;
using QrAttendanceApi.Infrastructure.ExternalServices;
using QrAttendanceApi.Infrastructure.ExternalServices.Emails;
using QrAttendanceApi.Infrastructure.ExternalServices.Uploads;
using QrAttendanceApi.Infrastructure.Persistence;
using QrAttendanceApi.Infrastructure.Repositories;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Text;

namespace QrAttendanceApi.Core.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer()
                .ConfigureSwaggerGen()
                .ConfigureDbContext(configuration)
                .ConfigureVersioning()
                .ConfigureServices(configuration)
                .ConfigureHangfire(configuration)
                .ConfigureJwt(configuration)
                .ConfigureCors(configuration);
        }

        public static IServiceCollection ConfigureServices(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            //Mailjet config
            var mailSection = configuration.GetSection("MailJet");
            services.Configure<MailJetSettings>(configuration.GetSection("MailJet"));
            var mailJetSettings = mailSection.Get<MailJetSettings>() ?? throw new ArgumentNullException("MailJetSettings");
            services.AddHttpClient<IMailjetClient, MailjetClient>(opt =>
            {
                opt.UseBasicAuthentication(mailJetSettings.ApiKey, mailJetSettings.ApiSecret);
            });

            //Cloudinary config
            services.Configure<CloudinarySettings>(configuration
                .GetSection("CloudinarySettings"));

            return services.AddScoped<IServiceManager, ServiceManager>()
                .AddScoped<IRepositoryManager, RepositoryManager>()
                .AddScoped<IUploadService, UploadService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IAuditEntry, AuditEntry>();
        }

        private static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {  
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }

        public static void ConfigireSerilogEsSink(this IHostBuilder host, IConfiguration configuration)
        {
            var url = configuration["ElasticSearch:Url"] ?? throw new ArgumentNullException(nameof(ElasticsearchSettings));

            host.UseSerilog((context, services, configuration) =>
            {
                var env = context.HostingEnvironment;
                var appName = env.ApplicationName;
                var indexFormat = $"{appName?.ToLower().Replace(".", "-")}-{env.EnvironmentName.ToLower()}";

                configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("Environment", env.EnvironmentName)
                    .Enrich.WithProperty("Service", appName)
                    .MinimumLevel.Information()
                    .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                    .WriteTo.Elasticsearch(ConfigureElasticSink(url, indexFormat));
            });
        }

        private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default") ?? 
                throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<AppDbContext>(opt => 
                opt.UseNpgsql(connectionString, m => m.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequiredLength = 8;

                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection ConfigureVersioning(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(opt =>
            {
                opt.GroupNameFormat = "'v'VVV";
                opt.SubstituteApiVersionInUrl = true;
            });

            return services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new HeaderApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new UrlSegmentApiVersionReader()
                );
            });
        }

        private static IServiceCollection ConfigureSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = "JWT Authentication",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Smart Attendance System API",
                    Version = "v1",
                    Description = "Smart Attendance System API v1.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Conclase .NET Cohort 8",
                        Email = "academytutor02@conclaseint.com"
                    }
                });

                opt.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Smart Attendance System API",
                    Version = "v2",
                    Description = "QR Attendance API v2.0",
                    Contact = new OpenApiContact
                    {
                        Name = "Conclase .NET Cohort 8",
                        Email = "academytutor02@conclaseint.com"
                    }
                });
            });

            return services;
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(string elasticSearchUrl, string indexFormat)
        {
            return new ElasticsearchSinkOptions(new Uri(elasticSearchUrl))
            {
                AutoRegisterTemplate = false,
                IndexFormat = indexFormat,
                NumberOfReplicas = 1,
                NumberOfShards = 2
            };
        }

        private static IServiceCollection ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Jwt");
            services.Configure<JwtSettings>(section);

            var settings = section.Get<JwtSettings>() ?? 
                throw new ArgumentNullException(nameof(section));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = settings.Audience,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key))
                };
            });

            services.AddAuthorization();

            return services;
        }

        private static IServiceCollection ConfigureHangfire(this IServiceCollection services,
                                                           IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UsePostgreSqlStorage(opt =>
                    {
                        opt.UseNpgsqlConnection(configuration.GetConnectionString("Default"));
                    }, 
                    new PostgreSqlStorageOptions
                    {
                        SchemaName = "smart-attendance-hangfire",
                        PrepareSchemaIfNecessary = true
                    })
                    .UseConsole()
                    .UseFilter(new AutomaticRetryAttribute()
                    {
                        Attempts = 5,
                        DelayInSecondsByAttemptFunc = _ => 60
                    });
            }).AddHangfireServer(opt =>
            {
                opt.ServerName = "Smart Attendance Hangfire Server";
                opt.Queues = new[] { "recurring", "default" };
                opt.SchedulePollingInterval = TimeSpan.FromMinutes(1);
                opt.WorkerCount = 5;
            });

            return services;
        }
    }
}