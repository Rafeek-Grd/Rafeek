using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetAcademicServiceInitialData
{
    public class GetAcademicServiceInitialDataQueryHandler : IRequestHandler<GetAcademicServiceInitialDataQuery, AcademicServiceInitialDataDto>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetAcademicServiceInitialDataQueryHandler(IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<AcademicServiceInitialDataDto> Handle(GetAcademicServiceInitialDataQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var student = await _context.Students
                .AsNoTracking()
                .Include(s => s.User)
                .Include(s => s.Department)
                .Include(s => s.AcademicAdvisor)
                    .ThenInclude(a => a != null ? a.User : null)
                .Include(s => s.AcademicAdvisor)
                    .ThenInclude(a => a != null ? a.Department : null)
                .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            if (student == null)
            {
                throw new NotFoundException(nameof(Student), userId);
            }

            var advisorName = "غير محدد";
            var advisorOffice = "غير محدد";

            if (student.AcademicAdvisor != null)
            {
                advisorName = student.AcademicAdvisor.User?.FullName ?? "غير محدد";
                advisorOffice = student.AcademicAdvisor.Department?.Name ?? "المكتب الأكاديمي";
            }

            return new AcademicServiceInitialDataDto
            {
                StudentName = student.User.FullName,
                UniversityCode = student.UniversityCode,
                DepartmentName = student.Department?.Name ?? "غير محدد",
                AdvisorName = advisorName,
                AdvisorLocation = advisorOffice
            };
        }
    }
}
