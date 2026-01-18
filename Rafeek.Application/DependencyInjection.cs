using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Behaviours;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Services;
using Rafeek.Application.Localization;
using System.Reflection;

namespace Rafeek.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddFluentValidation(conf =>
            {
                conf.DisableDataAnnotationsValidation = true;
                conf.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            services.AddTransient(typeof(Lazy<>), typeof(LazilyResolved<>));

            services.AddSingleton<FileExtensionContentTypeProvider>(sp => new FileExtensionContentTypeProvider());
            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            services.AddScoped<IImageValidator, ImageValidator>();
            services.AddScoped<IVideoValidator, VideoValidator>();
            services.AddScoped<IAudioValidator, AudioValidator>();
            services.AddScoped<IFileValidator, FileValidator>();

            services.AddSingleton<LocalizationManager>();

            UploadPaths.Configure(configuration);

            return services;
        }
    }
}