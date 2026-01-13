using Rafeek.Application;
using Rafeek.Infrastructure;
using Rafeek.Persistence;
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

            builder.Services.AddOptions();

            #endregion

            #region Add services to DI container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add libraries services
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure();
            builder.Services.AddPersistence();

            #endregion


            var app = builder.Build();

            #region Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI();

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
