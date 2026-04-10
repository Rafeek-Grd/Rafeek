using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands
{
    public class UpdateAcademicTermCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public TermType TermType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? RegistrationStartDate { get; set; }
        public DateTime? RegistrationEndDate { get; set; }
        public DateTime? DropDeadline { get; set; }
        public DateTime? ExamStartDate { get; set; }
        public DateTime? ExamEndDate { get; set; }
        public Guid AcademicYearId { get; set; }
    }

    public class UpdateAcademicTermCommandHandler : IRequestHandler<UpdateAcademicTermCommand, bool>
    {
        private readonly IRafeekDbContext _context;

        public UpdateAcademicTermCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateAcademicTermCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.AcademicTerms.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                return false;
            }

            entity.Name = request.Name;
            entity.TermType = request.TermType;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.RegistrationStartDate = request.RegistrationStartDate;
            entity.RegistrationEndDate = request.RegistrationEndDate;
            entity.DropDeadline = request.DropDeadline;
            entity.ExamStartDate = request.ExamStartDate;
            entity.ExamEndDate = request.ExamEndDate;
            entity.AcademicYearId = request.AcademicYearId;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
