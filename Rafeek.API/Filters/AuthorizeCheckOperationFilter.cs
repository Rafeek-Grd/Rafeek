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
            var actionAttrs = context.MethodInfo.GetCustomAttributes(inherit: true).OfType<object>();
            var controllerAttrs = context.MethodInfo.DeclaringType?.GetCustomAttributes(inherit: true).OfType<object>() ?? Enumerable.Empty<object>();

            // If AllowAnonymous is present on action or controller, remove any security and return
            var hasAllowAnonymous = actionAttrs.OfType<AllowAnonymousAttribute>().Any() || controllerAttrs.OfType<AllowAnonymousAttribute>().Any();
            if (hasAllowAnonymous)
            {
                operation.Security = null;
                return;
            }

            // If neither action nor controller has Authorize, remove security and return
            var hasAuthorize = actionAttrs.OfType<AuthorizeAttribute>().Any() || controllerAttrs.OfType<AuthorizeAttribute>().Any()
                              || actionAttrs.OfType<RoleAuthorizeAttribute>().Any() || controllerAttrs.OfType<RoleAuthorizeAttribute>().Any();
            if (!hasAuthorize)
            {
                operation.Security = null;
                return;
            }

            // Action/controller is protected: attach the Bearer requirement
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            // Avoid duplicate entries
            if (!operation.Security.Any(r => r.Keys.Any(k => k.Reference?.Id == "Bearer")))
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [bearerScheme] = new string[] { }
                });
            }
        }
    }
}