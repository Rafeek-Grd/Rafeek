using AutoMapper;
using MediatR;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands
{
    public class CreateAcademicYearCommand : IRequest<Guid>
    {
        public string Name { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrentYear { get; set; }
    }

    public class CreateAcademicYearCommandHandler : IRequestHandler<CreateAcademicYearCommand, Guid>
    {
        private readonly IRafeekDbContext _context;
        private readonly IMapper _mapper;

        public CreateAcademicYearCommandHandler(IRafeekDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateAcademicYearCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<AcademicYear>(request);

            _context.AcademicYears.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
