using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommandHandler : IRequestHandler<DeleteDepartmentCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteDepartmentCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<string> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            var department = await _ctx.DepartmentRepository.GetBy(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            department.IsActive = false;
            department.IsDeleted = true;

            _ctx.DepartmentRepository.Update(department);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ?
                _localizer[LocalizationKeys.GlobalValidationMessages.DeletedSuccessfully.Value]:
                _localizer[LocalizationKeys.GlobalValidationMessages.DeletedFailed.Value];
        }
    }
}
