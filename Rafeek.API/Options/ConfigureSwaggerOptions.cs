using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Rafeek.Application.Common.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rafeek.API.Options
{
    /// <summary>
    /// Configures Swagger documentation for each discovered API version
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _configuration;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var swaggerDocOptions = new SwaggerDocOptions();
            _configuration.GetSection("SwaggerDocOptions").Bind(swaggerDocOptions);

            // Create a Swagger doc for each discovered API version
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = swaggerDocOptions.Title,
                    Version = description.ApiVersion.ToString(),
                    Description = swaggerDocOptions.Description,
                    Contact = new OpenApiContact
                    {
                        Name = swaggerDocOptions.Organization,
                        Email = swaggerDocOptions.Email
                    }
                });
            }

            // Use ApiExplorer's GroupName to include actions in the correct doc
            options.DocInclusionPredicate((docName, apiDesc) =>
            {
                return string.Equals(apiDesc.GroupName, docName, StringComparison.OrdinalIgnoreCase);
            });
        }
    }
}
