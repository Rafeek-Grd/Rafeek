using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetStudentGrades
{
    public class GetStudentGradesQueryHandler : IRequestHandler<GetStudentGradesQuery, StudentAIGradesDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRafeekDbContext _dbContext;

        public GetStudentGradesQueryHandler(IUnitOfWork unitOfWork, IRafeekDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<StudentAIGradesDto> Handle(GetStudentGradesQuery request, CancellationToken cancellationToken)
        {
            var student = await _unitOfWork.StudentRepository
                .GetAll(s => s.Id == request.StudentId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (student == null)
            {
                return new StudentAIGradesDto();
            }

            var profile = await _unitOfWork.StudentAcademicProfileRepository
                .GetAll(p => p.Id == student.AcademicProfileId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            var result = new StudentAIGradesDto
            {
                StudentId = student.UniversityCode,
                GPA = profile?.CGPA ?? 0
            };

            var enrollments = await _dbContext.Enrollments
                .AsNoTracking()
                .Where(e => e.StudentId == student.Id)
                .Include(e => e.Course)
                .Include(e => e.Grades)
                .ToListAsync(cancellationToken);

            foreach (var enrollment in enrollments)
            {
                var score = enrollment.Grades
                    .OrderByDescending(g => g.CreatedAt)
                    .Select(g => g.AbsoluteScore)
                    .FirstOrDefault();

                result.CourseGrades[enrollment.Course.Code] = score;
            }

            return result;
        }
    }
}
