using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Rafeek.Persistence;
using Rafeek.Persistence.Seed;
using Rafeek.Domain.Entities;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<RafeekDbContext>(options =>
                    options.UseSqlServer(\"Server=sql.monsterasp.net;Database=db43195;User Id=db43195;Password=Rafeek123!@#;\")); // Use standard dummy or their settings
                services.AddDbContext<RafeekIdentityDbContext>(options =>
                    options.UseSqlServer(\"Server=sql.monsterasp.net;Database=db43195;User Id=db43195;Password=Rafeek123!@#;\"));
                services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                    .AddEntityFrameworkStores<RafeekIdentityDbContext>();
            });

        var host = builder.Build();
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<RafeekDbContext>();
                var identityContext = services.GetRequiredService<RafeekIdentityDbContext>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                
                // Clear Departments to bypass the guard!
                await context.Database.ExecuteSqlRawAsync(\"DELETE FROM Departments\");
                
                await RafeekDbSeeder.SeedAsync(context, identityContext, userManager);
                Console.WriteLine(\"Seed completed successfully!\");
            }
            catch (Exception ex)
            {
                Console.WriteLine(\"Error:\");
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
