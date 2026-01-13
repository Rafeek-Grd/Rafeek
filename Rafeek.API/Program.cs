using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.OpenApi.Models;
using Rafeek.API.Filters;
using Rafeek.API.Options;
using Rafeek.Application;
using Rafeek.Application.Common.Options;
using Rafeek.Infrastructure;
using Rafeek.Persistence;
using System.Globalization;
using System.Reflection;

namespace Rafeek.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Application Configuration

            var env = builder.Environment;

            // Clear and rebuild configuration
            builder.Configuration.Sources.Clear();

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            if (env.EnvironmentName == "Development" || env.EnvironmentName == "Live")
            {
                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                if (appAssembly != null)
                {
                    builder.Configuration.AddUserSecrets(appAssembly, optional: true);
                }
            }

            builder.Configuration.AddEnvironmentVariables()
                                 .AddCommandLine(args);

            #endregion

            #region Add services to DI container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Register API versioning with proper configuration
            builder.Services.AddApiVersioning(options =>
            {
                // Treat controllers without version as the default version
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                // Substitute the version in route URLs where {version:apiVersion} is used
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddLocalization(options => options.ResourcesPath = "Localization/Resources");

            builder.Services.AddMvc()
              .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
              .ConfigureApiBehaviorOptions(options =>
              {
                  // Suppress automatic 400 responses - allows custom validation handling
                  options.SuppressModelStateInvalidFilter = true;
              });


            // Configure Swagger with versioning
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

            // Configure Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            // Add libraries services
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            builder.Services.AddPersistence();

            // Add options pattern support
            builder.Services.AddOptions();

            // Configure HSTS (HTTP Strict Transport Security)
            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(365);
            });

            // Configure CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", cors =>
                {
                    cors.SetIsOriginAllowed(_ => true)
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            #endregion

            var app = builder.Build();

            #region Configure the HTTP request pipeline.

            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            var swaggerDocOptions = new SwaggerDocOptions();
            app.Configuration.GetSection("SwaggerDocOptions").Bind(swaggerDocOptions);

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                // Order by major, then minor to ensure highest version is first
                foreach (var description in provider.ApiVersionDescriptions
                                                 .OrderByDescending(d => (d.ApiVersion.MajorVersion, d.ApiVersion.MinorVersion)))
                {
                    var endpoint = $"/swagger/{description.GroupName}/swagger.json";
                    var name = $"{swaggerDocOptions.Title} {description.GroupName.ToUpperInvariant()}";

                    options.SwaggerEndpoint(endpoint, name);
                }
            });

            // Configure localization
            var supportedCultures = new[]
            {
                new CultureInfo("ar"),
                new CultureInfo("en")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ar"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Only use HTTPS redirection in Development (Railway handles HTTPS at proxy level)
            if (app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}