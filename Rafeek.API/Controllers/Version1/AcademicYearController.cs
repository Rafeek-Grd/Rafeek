using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Localization;
using Rafeek.Application.Handlers.AcademicYearHandlers.Commands;
using Rafeek.Application.Handlers.AcademicYearHandlers.Queries;
using System;
using System.Threading.Tasks;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class AcademicYearController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AcademicYearController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route(ApiRoutes.AcademicYear.Create)]
        public async Task<IActionResult> Create([FromBody] CreateAcademicYearCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route(ApiRoutes.AcademicYear.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAcademicYearCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        [Route(ApiRoutes.AcademicYear.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeleteAcademicYearCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.AcademicYear.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllAcademicYearsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route(ApiRoutes.AcademicYear.GetById)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetAcademicYearByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
