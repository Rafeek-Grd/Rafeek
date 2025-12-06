using Rafeek.Application;
using Rafeek.Infrastructure;
using Rafeek.Persistence;

namespace Rafeek.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
