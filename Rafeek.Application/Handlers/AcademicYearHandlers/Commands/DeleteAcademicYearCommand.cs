using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands
{
    public class DeleteAcademicYearCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAcademicYearCommandHandler : IRequestHandler<DeleteAcademicYearCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public DeleteAcademicYearCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.AcademicYears.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return false;
            }

            _context.AcademicYears.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
