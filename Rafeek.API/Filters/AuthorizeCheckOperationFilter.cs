using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rafeek.API.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Safety: ensure operation object exists
            if (operation == null || context?.MethodInfo == null) return;

            // Gather attributes (true => include inherited attributes from base classes)
            var actionAttrs = context.MethodInfo.GetCustomAttributes(inherit: true).ToList();
            var controllerAttrs = context.MethodInfo.DeclaringType?.GetCustomAttributes(inherit: true).ToList() ?? new List<object>();

            // If AllowAnonymous is present on action or controller, remove any security and return
            var hasAllowAnonymous = actionAttrs.OfType<AllowAnonymousAttribute>().Any() || controllerAttrs.OfType<AllowAnonymousAttribute>().Any();
            if (hasAllowAnonymous)
            {
                operation.Security = null;
                return;
            }

            // Check for Authorize or ApiKey attributes
            var hasAuthorize = actionAttrs.OfType<AuthorizeAttribute>().Any() || controllerAttrs.OfType<AuthorizeAttribute>().Any()
                              || actionAttrs.OfType<RoleAuthorizeAttribute>().Any() || controllerAttrs.OfType<RoleAuthorizeAttribute>().Any();
            
            var hasApiKey = actionAttrs.OfType<ApiKeyAttribute>().Any() || controllerAttrs.OfType<ApiKeyAttribute>().Any();

            if (!hasAuthorize && !hasApiKey)
            {
                operation.Security = null;
                return;
            }

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            // Attach Bearer requirement if authorized
            if (hasAuthorize)
            {
                var bearerScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };

                if (!operation.Security.Any(r => r.Keys.Any(k => k.Reference?.Id == "Bearer")))
                {
                    operation.Security.Add(new OpenApiSecurityRequirement { [bearerScheme] = new string[] { } });
                }
            }

            // Attach AiApiKey requirement if ApiKey is used
            if (hasApiKey)
            {
                var apiKeyScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "AiApiKey" }
                };

                if (!operation.Security.Any(r => r.Keys.Any(k => k.Reference?.Id == "AiApiKey")))
                {
                    operation.Security.Add(new OpenApiSecurityRequirement { [apiKeyScheme] = new string[] { } });
                }
            }
        }
    }
}