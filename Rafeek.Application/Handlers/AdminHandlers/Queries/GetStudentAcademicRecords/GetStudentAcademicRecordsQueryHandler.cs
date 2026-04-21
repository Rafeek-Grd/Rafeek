using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentAcademicRecords
{
    public class GetStudentAcademicRecordsQueryHandler
        : IRequestHandler<GetStudentAcademicRecordsQuery, PagginatedResult<StudentAcademicRecordDto>>
    {
        private readonly IRafeekDbContext _context;

        public GetStudentAcademicRecordsQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<PagginatedResult<StudentAcademicRecordDto>> Handle(
            GetStudentAcademicRecordsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Students
                .AsNoTracking()
                .Include(s => s.AcademicProfile)
                .Include(s => s.Department)
                .Include(s => s.User)
                .AsQueryable();

            // ── Filtering ────────────────────────────────────────────────────────
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();
                query = query.Where(s =>
                    s.User.FullName.Contains(term) ||
                    s.User.Email!.Contains(term) ||
                    s.UniversityCode.Contains(term));
            }

            if (request.DepartmentId.HasValue)
                query = query.Where(s => s.DepartmentId == request.DepartmentId);

            // الحالة الأكاديمية بناءً على المعدل التراكمي
            if (!string.IsNullOrWhiteSpace(request.AcademicStatus))
            {
                query = request.AcademicStatus.Trim().ToLower() switch
                {
                    "stable"    => query.Where(s => s.AcademicProfile != null && s.AcademicProfile.CGPA >= 2.0f),
                    "warning"   => query.Where(s => s.AcademicProfile != null && s.AcademicProfile.CGPA >= 1.0f && s.AcademicProfile.CGPA < 2.0f),
                    "probation" => query.Where(s => s.AcademicProfile != null && s.AcademicProfile.CGPA < 1.0f),
                    _           => query
                };
            }

            // ── Count ────────────────────────────────────────────────────────────
            var totalCount = await query.CountAsync(cancellationToken);

            // ── Paginate & Project ───────────────────────────────────────────────
            var items = await query
                .OrderBy(s => s.User.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new StudentAcademicRecordDto
                {
                    StudentId        = s.Id,
                    FullName         = s.User.FullName,
                    UniversityEmail  = s.User.Email!,
                    UniversityCode   = s.UniversityCode,
                    DepartmentName   = s.Department != null ? s.Department.Name : null,
                    Cgpa             = s.AcademicProfile != null ? s.AcademicProfile.CGPA : 0f,
                    Level            = s.Level,
                    Term             = s.Term,
                    AcademicStatus   = s.AcademicProfile == null ? "Stable"
                                     : s.AcademicProfile.CGPA >= 2.0f ? "Stable"
                                     : s.AcademicProfile.CGPA >= 1.0f ? "Warning"
                                     : "Probation",
                    AcademicStatusLabel = s.AcademicProfile == null ? "منتظم"
                                        : s.AcademicProfile.CGPA >= 2.0f ? "منتظم"
                                        : s.AcademicProfile.CGPA >= 1.0f ? "تحذير"
                                        : "إنذار أول"
                })
                .ToListAsync(cancellationToken);

            return PagginatedResult<StudentAcademicRecordDto>.Create(
                items.AsReadOnly(),
                totalCount,
                request.PageNumber,
                request.PageSize);
        }
    }
}
