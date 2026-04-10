using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.Application.Handlers.AcademicTermHandlers.Commands;
using Rafeek.Application.Handlers.AcademicTermHandlers.Queries;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin")]
    public class AcademicTermController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AcademicTermController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route(ApiRoutes.AcademicTerm.Create)]
        public async Task<IActionResult> Create([FromBody] CreateAcademicTermCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route(ApiRoutes.AcademicTerm.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAcademicTermCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Route(ApiRoutes.AcademicTerm.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteAcademicTermCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.AcademicTerm.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAcademicTermsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.AcademicTerm.GetById)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetAcademicTermByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
