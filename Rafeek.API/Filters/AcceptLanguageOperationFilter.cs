using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rafeek.API.Swagger
{
    public class AcceptLanguageOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            if (!operation.Parameters.Any(p => p.Name.Equals("Accept-Language", StringComparison.OrdinalIgnoreCase)))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Accept-Language",
                    In = ParameterLocation.Header,
                    Description = "Optional. Can Be Local override.",
                    Required = false,
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }
        }
    }
}