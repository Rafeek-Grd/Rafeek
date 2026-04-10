using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands
{
    public class CreateAcademicTermCommand : IRequest<Guid>
    {
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

    public class CreateAcademicTermCommandHandler : IRequestHandler<CreateAcademicTermCommand, Guid>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public CreateAcademicTermCommandHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateAcademicTermCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<AcademicTerm>(request);

            _context.AcademicTerms.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
