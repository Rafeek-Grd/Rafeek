using System.Reflection;
using MediatR;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rafeek.API.Filters
{
    public class RegisterCommandSchemasDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var appAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && (a.GetName().Name?.StartsWith("Rafeek.Application") == true || a.GetName().Name?.StartsWith("Rafeek.Domain") == true))
                .ToArray();

            foreach (var assembly in appAssemblies)
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types.Where(t => t != null).ToArray()!;
                }

                foreach (var type in types)
                {
                    if (type == null) continue;
                    if (!type.IsClass || type.IsAbstract || !type.IsPublic) continue;

                    var interfaces = type.GetInterfaces();
                    var isRequest = interfaces.Any(i =>
                        (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
                        || i == typeof(IRequest));

                    if (!isRequest) continue;

                    var typeName = type.Name;
                    if (!context.SchemaRepository.Schemas.ContainsKey(typeName))
                    {
                        // Generate schema and register in SchemaRepository
                        context.SchemaGenerator.GenerateSchema(type, context.SchemaRepository);
                    }
                }
            }
        }
    }
}