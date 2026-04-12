using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateDepartmentCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<Unit> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _ctx.DepartmentRepository
                .GetBy(d => d.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            department.Name = request.Name;
            department.Description = request.Description;
            department.IsActive = request.IsActive.Value;
            department.IsDeleted = request.IsDeleted.Value;

            _ctx.DepartmentRepository.Update(department);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            if(result > 0)
            {
                return Unit.Value;
            }
            else
            {
                throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.UpdatedFailed.Value]);
            }
        }
    }
}
