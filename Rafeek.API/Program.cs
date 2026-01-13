using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Rafeek.API.Filters;
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

            // Register API versioning BEFORE Swagger configuration
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                // Format: "v1", "v2", etc.
                options.GroupNameFormat = "'v'VVV";
                // Substitute the version in route URLs where {version:apiVersion} is used.
                options.SubstituteApiVersionInUrl = true;
            });

            var swaggerDocOptions = new SwaggerDocOptions();
            builder.Configuration.GetSection("SwaggerDocOptions").Bind(swaggerDocOptions);

            // Configure Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                var provider = builder.Services.BuildServiceProvider()
                                               .GetRequiredService<IApiVersionDescriptionProvider>();

                // Use description.GroupName so document names match the UI endpoints
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = swaggerDocOptions.Title,
                        Version = description.ApiVersion.ToString(),
                        Description = swaggerDocOptions.Description,
                        Contact = new OpenApiContact
                        {
                            Name = swaggerDocOptions.Organization
                        }
                    });
                }

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.OperationFilter<AuthorizeCheckOperationFilter>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
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
                options.MaxAge = TimeSpan.FromDays(1);
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

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                            $"{swaggerDocOptions.Title} {description.GroupName.ToUpperInvariant()}");
                }
            });

            var supportedCultures = new[]
            {
                new CultureInfo("ar")
            };

            // Configure localization
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

            app.UseAuthorization();
            app.MapControllers();

            #endregion

            app.Run();
        }
    }
}