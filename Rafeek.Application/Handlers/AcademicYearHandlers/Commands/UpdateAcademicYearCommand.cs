using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands
{
    public class UpdateAcademicYearCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentYear { get; set; }
    }

    public class UpdateAcademicYearCommandHandler : IRequestHandler<UpdateAcademicYearCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public UpdateAcademicYearCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.AcademicYears.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return false;
            }

            entity.Name = request.Name;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.IsCurrentYear = request.IsCurrentYear;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
