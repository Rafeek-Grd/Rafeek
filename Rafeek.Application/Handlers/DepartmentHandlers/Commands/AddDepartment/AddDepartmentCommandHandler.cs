using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AddDepartment
{
    public class AddDepartmentCommandHandler : IRequestHandler<AddDepartmentCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public AddDepartmentCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(AddDepartmentCommand request, CancellationToken cancellationToken)
        {
            var newDepartment = new Department()
            {
                Name = request.Name,
                Code = request.Code,
                Description = request.Description
            };

            await _ctx.DepartmentRepository.AddAsync(newDepartment);
            var result = await _ctx.SaveChangesAsync(cancellationToken);
            if (result > 0)
            {
                return Unit.Value;
            }
            else
            {
                throw new BadRequestException("Failed to add department");
            }
        }
    }
}
