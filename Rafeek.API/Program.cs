using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using NLog.Web;
using Rafeek.API.Filters;
using Rafeek.API.Options;
using Rafeek.API.Services;
using Rafeek.API.Swagger;
using Rafeek.Application;
using Rafeek.Application.Common.Options;
using Rafeek.Application.Localization;
using Rafeek.Infrastructure;
using Rafeek.Persistence;
using System.Globalization;
using System.Reflection;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Stores;
using tusdotnet.Models.Configuration;

namespace Rafeek.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // Replace your NLog setup with this:
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            try
            {
                logger.Info("Application starting up");

                var builder = WebApplication.CreateBuilder(args);

                // Clear default logging providers and configure NLog
                builder.Logging.ClearProviders();
                builder.Logging.SetMinimumLevel(LogLevel.Trace);
                builder.Logging.AddConsole();
                builder.Host.UseNLog();

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


                builder.Services.AddControllers(options =>
                {
                    options.Filters.Add<ApiExceptionFilterAttribute>();

                    // Add size limits for large file uploads
                    options.MaxModelValidationErrors = 50;
                }).AddNewtonsoftJson(options =>
                {
                    // Example: use camelCase and ignore nulls — adapt to your project's conventions
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                });
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

                builder.Services.AddLocalization();

                builder.Services.AddMvc()
                  .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                  .ConfigureApiBehaviorOptions(options =>
                  {
                      // Suppress automatic 400 responses - allows custom validation handling
                      options.SuppressModelStateInvalidFilter = true;
                  });


                builder.Services.Configure<KestrelServerOptions>(options =>
                {
                    options.Limits.MaxRequestBodySize = 250_000_000; // 250 MB
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
                    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
                });

                builder.Services.Configure<FormOptions>(options =>
                {
                    options.ValueLengthLimit = int.MaxValue;
                    options.MultipartBodyLengthLimit = 250 * 1024 * 1024; // 250 MB
                    options.MultipartHeadersLengthLimit = int.MaxValue;
                    options.BufferBodyLengthLimit = 250 * 1024 * 1024; // 250 MB
                });

                // Configure Swagger with versioning
                builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

                // Configure Swagger
                builder.Services.AddSwaggerGen(options =>
                {
                    options.OperationFilter<AcceptLanguageOperationFilter>();

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

                    options.MapType<IFormFile>(() => new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    });

                    options.MapType<List<IFormFile>>(() => new OpenApiSchema
                    {
                        Type = "array",
                        Items = new Microsoft.OpenApi.Models.OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }
                    });

                    options.DocumentFilter<RegisterCommandSchemasDocumentFilter>();

                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                });

                // Add libraries services
                builder.Services.AddApplication(builder.Configuration);
                builder.Services.AddInfrastructure();
                builder.Services.AddPersistence();

                // Add options pattern support
                builder.Services.AddOptions();

                builder.Services.AddMemoryCache();

                builder.Services.AddInMemoryRateLimiting();
                // load general configuration from appsettings.json
                builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

                // inject counter and rules stores
                builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
                builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

                // configuration (resolvers, counter key builders)
                builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
                builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

                builder.Services.Configure<SwaggerOptions>(builder.Configuration.GetSection("SwaggerOptions"));

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



                // Configure Response Compression
                builder.Services.AddResponseCompression(options =>
                {
                    options.EnableForHttps = true;
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });

                builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
                {
                    options.Level = System.IO.Compression.CompressionLevel.Fastest;
                });

                builder.Services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = System.IO.Compression.CompressionLevel.SmallestSize;
                });

                var app = builder.Build();


                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                var swaggerDocOptions = new SwaggerDocOptions();
                app.Configuration.GetSection("SwaggerDocOptions").Bind(swaggerDocOptions);

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                    // Read configured route template once
                    var swaggerOptions = app.Services.GetRequiredService<IOptions<SwaggerOptions>>().Value;
                    var jsonRouteTemplate = swaggerOptions?.JsonRoute?.Trim() ?? string.Empty;

                    // Order by major, then minor to ensure highest version is first
                    foreach (var description in provider.ApiVersionDescriptions
                                                        .OrderByDescending(d => (d.ApiVersion.MajorVersion, d.ApiVersion.MinorVersion)))
                    {
                        string endpoint;

                        if (!string.IsNullOrEmpty(jsonRouteTemplate))
                        {
                            if (jsonRouteTemplate.Contains("{0}"))
                                endpoint = string.Format(jsonRouteTemplate, description.GroupName);
                            else if (jsonRouteTemplate.Contains("{documentName}"))
                                endpoint = jsonRouteTemplate.Replace("{documentName}", description.GroupName);
                            else
                                endpoint = jsonRouteTemplate;
                        }
                        else
                            endpoint = $"/swagger/{description.GroupName}/swagger.json";

                        if (!endpoint.StartsWith("/"))
                            endpoint = "/" + endpoint;

                        var swaggerDocOptions = new SwaggerDocOptions();
                        app.Configuration.GetSection("SwaggerDocOptions").Bind(swaggerDocOptions);
                        var name = $"{swaggerDocOptions.Title} {description.GroupName.ToUpperInvariant()}";
                        options.SwaggerEndpoint(endpoint, name);

                        options.InjectJavascript("/swagger/swagger-ui/language.js");
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

                using (var scope = app.Services.CreateScope())
                {
                    var localizerFactory = scope.ServiceProvider.GetRequiredService<IStringLocalizerFactory>();
                    LocalizationManager.Configure(localizerFactory);
                }

                app.UseResponseCompression();

                app.UseXContentTypeOptions();
                app.UseXXssProtection(options => options.EnabledWithBlockMode());
                app.UseXfo(options => options.SameOrigin());
                app.UseReferrerPolicy(options => options.NoReferrerWhenDowngrade());

                app.UseIpRateLimiting();

                // Only use HTTPS redirection in Development (Railway handles HTTPS at proxy level)
                if (app.Environment.IsDevelopment())
                {
                    app.UseHttpsRedirection();
                }

                app.UseAuthentication();

                app.UseStaticFiles();

                 app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new CustomFileProvider(app.Environment.WebRootPath),
                    RequestPath = "/files"
                });

                // Configure TUS (Resumable File Uploads)
                app.UseTus(context => new DefaultTusConfiguration
                {
                    // Where to store the files
                    Store = new TusDiskStore(Path.Combine(env.WebRootPath, "tusfiles")),
                    // The URL path to listen for uploads
                    UrlPath = "/files",
                    Events = new Events
                    {
                        OnFileCompleteAsync = async eventContext =>
                        {
                            var file = await eventContext.GetFileAsync();
                            var logger = eventContext.HttpContext.RequestServices.GetService<ILogger<Program>>();
                            logger?.LogInformation($"Upload completed: {file.Id}");
                        }
                    }
                });

                app.UseRouting();

                app.UseCors("CorsPolicy");

                app.UseAuthorization();

                app.MapControllers();

                app.MapGet("/", () => Results.Redirect("/swagger"));

                app.Run();
            }
            catch (Exception ex)
            {
                // NLog: catch setup errors
                logger.Error(ex, "Application stopped because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }
}