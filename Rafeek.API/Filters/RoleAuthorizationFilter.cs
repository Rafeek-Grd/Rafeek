using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Localization;

namespace Rafeek.API.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles = Array.Empty<string>();

        // Keep constructors that can be used in attribute usage (no DI in attribute constructor)
        public RoleAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUserService = context.HttpContext.RequestServices.GetService(typeof(ICurrentUserService)) as ICurrentUserService;

            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous) return;

            var user = context.HttpContext.User;
            var localizer = context.HttpContext.RequestServices.GetRequiredService<IStringLocalizer<Messages>>();

            if (user.Identity == null || !user.Identity.IsAuthenticated || currentUserService == null || !currentUserService.IsAuthenticated)
            {
                var message = localizer[LocalizationKeys.ExceptionMessage.Unauthorized.Value];
                var response = ApiResponse<object>.Error(new Dictionary<string, string[]>(), message, StatusCodes.Status401Unauthorized);
                context.Result = new UnauthorizedObjectResult(response);
                return;
            }

            if (_roles.Any() && !_roles.Any(role => user.IsInRole(role)))
            {
                var message = localizer[LocalizationKeys.ExceptionMessage.Unauthorized.Value];
                var response = ApiResponse<object>.Error(new Dictionary<string, string[]>(), message, StatusCodes.Status403Forbidden);
                context.Result = new ObjectResult(response)
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
