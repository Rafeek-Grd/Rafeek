using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using NLog;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Localization;

namespace Rafeek.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;
        protected readonly IStringLocalizer<Messages>? _localizer;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ApiExceptionFilterAttribute(IStringLocalizer<Messages>? localizer)
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(BadRequestException), HandleBadRequestException },
                { typeof(UnauthorizedException), HandleUnauthorizedException },
            };
            _localizer = localizer;
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            var message = _localizer?[LocalizationKeys.ExceptionMessage.Validation.Value];
            var details = ApiResponse<object>.Error(exception.Errors, message, StatusCodes.Status400BadRequest);

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var errors = context.ModelState.ToDictionary(
                                kvp => kvp.Key,
                                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                            );

            var message = _localizer?[LocalizationKeys.ExceptionMessage.InvalidModelState.Value];
            var details = ApiResponse<object>.Error(errors, message, StatusCodes.Status400BadRequest);

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;
            var message = _localizer?[exception.Message ?? LocalizationKeys.ExceptionMessage.NotFound.Value];

            var details = ApiResponse<object>.Error(message, StatusCodes.Status404NotFound);

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleBadRequestException(ExceptionContext context)
        {
            var exception = context.Exception as BadRequestException;
            var message = _localizer?[exception.Message ?? LocalizationKeys.ExceptionMessage.BadRequest.Value];

            var details = ApiResponse<object>.Error(message, StatusCodes.Status400BadRequest);

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;

            _logger.Error(exception.Message);
        }

        private void HandleUnauthorizedException(ExceptionContext context)
        {
            var exception = context.Exception as UnauthorizedException;
            var message = _localizer?[exception.Message ?? LocalizationKeys.ExceptionMessage.Unauthorized.Value];

            var details = ApiResponse<object>.Error(new Dictionary<string, string[]>(), message, StatusCodes.Status401Unauthorized);

            context.Result = new UnauthorizedObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var message = _localizer?[LocalizationKeys.ExceptionMessage.UnknownException.Value];
            var details = ApiResponse<object>.Error(message, StatusCodes.Status500InternalServerError);

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            Console.WriteLine(context.Exception.Message);
            context.ExceptionHandled = true;
        }
    }
}