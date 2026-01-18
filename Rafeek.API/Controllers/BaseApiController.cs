using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers
{
    [ApiController]
    public class BaseApiController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<Messages>? _localizer;

        protected BaseApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected BaseApiController(IMediator mediator, IStringLocalizer<Messages> localizer)
        {
            _mediator = mediator;
            _localizer = localizer;
        }

        protected IActionResult Ok(string message) => base.Ok(ApiResponse<string>.Ok(null, message ?? _localizer[LocalizationKeys.AcionResultMessage.Ok.Value]));
        protected IActionResult Ok<TData>(TData? data, string message = null!) => base.Ok(ApiResponse<TData>.Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Ok.Value]));
        protected IActionResult Ok2<TData>(TData? data, string message = null!) => base.Ok(Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Ok.Value]));
        protected IActionResult Deleted<TData>(string uri, TData data, string message = null!) => base.Accepted(uri, ApiResponse<TData>.Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Deleted.Value]));
        protected IActionResult Accepted<TData>(string uri, TData data, string message = null!) => base.Accepted(uri, ApiResponse<TData>.Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Accepted.Value]));
        protected IActionResult Created<TData>(string uri, TData data, string message = null!) => base.Created(uri, ApiResponse<TData>.Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Created.Value]));
        protected IActionResult Deleted<TData>(TData data, string message = null!) => base.Accepted(ApiResponse<TData>.Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Deleted.Value]));
        protected IActionResult Accepted<TData>(TData data, string message = null!) => base.Accepted(ApiResponse<TData>.Ok(data, message ?? _localizer[LocalizationKeys.AcionResultMessage.Accepted.Value]));
    }
}
