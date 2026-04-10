using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands
{
    public class DeleteAcademicTermCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAcademicTermCommandHandler : IRequestHandler<DeleteAcademicTermCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public DeleteAcademicTermCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteAcademicTermCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.AcademicTerms.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return false;
            }

            _context.AcademicTerms.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
