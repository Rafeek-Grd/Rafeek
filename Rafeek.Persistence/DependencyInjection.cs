using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Register IdentityDbContext
            services.AddDbContext<RafeekIdentityDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("RafeekIdentityConnectionString"),
                    sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(RafeekIdentityDbContext).Assembly.GetName().Name);
                        sqlOptions.MigrationsHistoryTable("__IdentityMigrationsHistory", "auth");
                    });
            });

            // Register Application DbContext
            services.AddDbContext<RafeekDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("RafeekConnectionString"),
                    sqlOptions =>
                    {
                        // Use the assembly name (safer for migrations assembly resolution)
                        sqlOptions.MigrationsAssembly(typeof(RafeekDbContext).Assembly.GetName().Name);
                        sqlOptions.MigrationsHistoryTable("__AppMigrationsHistory", "dbo");
                    })
                .EnableSensitiveDataLogging() // disable for production
                .LogTo(Console.WriteLine, LogLevel.Information);
            }, ServiceLifetime.Scoped);

            services.AddScoped<IRafeekDbContext>(provider =>
            {
                var options = provider.GetRequiredService<DbContextOptions<RafeekDbContext>>();
                var currentUserService = provider.GetRequiredService<ICurrentUserService>();
                return new RafeekDbContext(options, currentUserService);
            });

            services.AddScoped<IRafeekIdentityDbContext>(provider =>
            {
                var options = provider.GetRequiredService<DbContextOptions<RafeekIdentityDbContext>>();
                var currentUserService = provider.GetRequiredService<ICurrentUserService>();
                return new RafeekIdentityDbContext(options, currentUserService);
            });

            return services;
        }
    }
}
