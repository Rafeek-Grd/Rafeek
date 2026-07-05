using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetStudentSupportById
{
    public class GetStudentSupportByIdQueryHandler : IRequestHandler<GetStudentSupportByIdQuery, NewStudentSupportDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetStudentSupportByIdQueryHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<NewStudentSupportDto> Handle(GetStudentSupportByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.StudentSupportRepository.FindByKeyAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            return new NewStudentSupportDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Email = entity.Email ?? string.Empty,
                StudentSupportStatus = entity.StudentSupportStatus,
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted
            };
        }
    }
}
