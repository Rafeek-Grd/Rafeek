using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Infrastructure.DataProtection;
using Rafeek.Infrastructure.Identity;
using Rafeek.Infrastructure.Oauth;
using Rafeek.Persistence;
using System.Text;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;
using DataProtectionOptions = Rafeek.Infrastructure.DataProtection.DataProtectionOptions;
using IdentityOptions = Rafeek.Infrastructure.Identity.IdentityOptions;
using Microsoft.AspNetCore.Http;
using Rafeek.Application.Localization;

namespace Rafeek.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Register JwtToken Manager
            services.AddTransient<IJwtTokenManager, JwtTokenManager>();
            services.AddTransient<ISignInManager, SignInManager>();

            // Register Generic Repositories
            services.AddScoped(typeof(IEntityRepository<,>), typeof(EntityRepository<,>));
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));

            // Configure Identity (Must be before Authentication to avoid overwriting defaults)
            var identityOptionsConfig = new IdentityOptions();
            configuration.Bind(nameof(IdentityOptions), identityOptionsConfig);

            services.AddIdentity<IdentityUser<Guid>, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireNonAlphanumeric = identityOptionsConfig.RequireNonAlphanumeric;
                options.Password.RequiredLength = identityOptionsConfig.RequiredLength;
                options.Password.RequireDigit = identityOptionsConfig.RequiredDigit;
                options.Password.RequireLowercase = identityOptionsConfig.RequireLowercase;
                options.Password.RequiredUniqueChars = identityOptionsConfig.RequiredUniqueChars;
                options.Password.RequireUppercase = identityOptionsConfig.RequireUppercase;
                options.Lockout.MaxFailedAccessAttempts = identityOptionsConfig.MaxFailedAttempts;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(identityOptionsConfig.LockoutTimeSpanInDays);
                options.SignIn.RequireConfirmedEmail = identityOptionsConfig.RequireConfirmedEmail;
                options.User.AllowedUserNameCharacters = identityOptionsConfig.AllowedUserNameCharacters;
                options.User.RequireUniqueEmail = identityOptionsConfig.RequireUniqueEmail;
            })
           .AddEntityFrameworkStores<RafeekIdentityDbContext>()
           .AddDefaultTokenProviders();

            // Configure JWT Authentication
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = jwtSettings.ValidateAudience,
                ValidAudience = jwtSettings.Audience,
                RequireExpirationTime = jwtSettings.RequireExpirationTime,
                ValidateLifetime = jwtSettings.ValidateLifetime,
                ClockSkew = jwtSettings.AccessTokenExpiration,
            };
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // You can customize the message here or resolve it from localization if needed
                        var result = System.Text.Json.JsonSerializer.Serialize(new
                        { 
                            succeeded = false, 
                            message = LocalizationKeys.ExceptionMessage.Unauthorized.Value, // Or use a localized string
                            errors = new Dictionary<string, string[]>(),
                            code = 401
                        });

                        return context.Response.WriteAsync(result);
                    }
                };
            });



            services.AddScoped<IIdentityUnitOfWork, IdentityUnitOfWork>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            var dataProtectionOptionsConfig = new DataProtectionOptions();
            configuration.GetSection("DataProtection").Bind(dataProtectionOptionsConfig);

            var lifetimeDays = Math.Max(dataProtectionOptionsConfig.KeyLifeTimeInDays, 7.0);
            services.AddDataProtection()
                .SetDefaultKeyLifetime(TimeSpan.FromDays(lifetimeDays));

            services.AddSingleton<IDataEncryption, RouteDataProtection>();

            return services;
        }
    }
}
