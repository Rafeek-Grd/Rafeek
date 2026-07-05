using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllActiveStudentSupportForCurrentUser
{
    public class GetAllActiveStudentSupportForCurrentUserQueryHandler : IRequestHandler<GetAllActiveStudentSupportForCurrentUserQuery, List<NewStudentSupportDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;

        public GetAllActiveStudentSupportForCurrentUserQueryHandler(IUnitOfWork ctx, ICurrentUserService currentUserService, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<List<NewStudentSupportDto>> Handle(GetAllActiveStudentSupportForCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var student = await _ctx.StudentRepository
                .GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.UserId == _currentUserService.UserId, cancellationToken)
                ?? throw new NotFoundException(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);

            return await _ctx.StudentSupportRepository
                .GetAll()
                .AsNoTracking()
                .Where(x => x.StudentId == student.Id && x.IsActive)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new NewStudentSupportDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Email = x.Email ?? string.Empty,
                    StudentSupportStatus = x.StudentSupportStatus,
                    IsActive = x.IsActive,
                    IsDeleted = x.IsDeleted
                })
                .ToListAsync(cancellationToken);
        }
    }
}
