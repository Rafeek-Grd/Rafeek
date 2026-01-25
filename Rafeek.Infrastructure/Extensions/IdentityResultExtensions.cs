using Microsoft.AspNetCore.Identity;
using Rafeek.Application.Common.Models;

namespace Rafeek.Infrastructure.Extensions
{
    public static class IdentityResultExtensions
    {
        public static Result MapToResult(this IdentityResult result)
        {
            return result.Succeeded
                ? result.Success()
                : result.Failure(result.Errors.Select(e => e.Description));
        }

        public static Result MapToResult(this IdentityResult result, object data)
        {
            return result.Succeeded
                ? result.Success(data)
                : result.Failure(result.Errors.Select(e => e.Description));
        }

        public static Result MapToResult(this SignInResult result)
        {
            return result.Succeeded
                ? result.Success()
                : result.Failure(new string[] { "Invalid Login Attempt." });
        }

        public static Result MapToResult(this SignInResult result, object data)
        {
            return result.Succeeded
                ? result.Success(data)
                : result.Failure(new string[] { "Invalid Login Attempt." });
        }
    }
}
