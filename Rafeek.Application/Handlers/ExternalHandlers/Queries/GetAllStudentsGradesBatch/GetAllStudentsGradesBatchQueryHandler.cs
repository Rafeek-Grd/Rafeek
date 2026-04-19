using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models.AI;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAllStudentsGradesBatch
{
    public class GetAllStudentsGradesBatchQueryHandler : IRequestHandler<GetAllStudentsGradesBatchQuery, List<BatchStudentAIGradesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRafeekDbContext _dbContext;

        public GetAllStudentsGradesBatchQueryHandler(IUnitOfWork unitOfWork, IRafeekDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<List<BatchStudentAIGradesDto>> Handle(GetAllStudentsGradesBatchQuery request, CancellationToken cancellationToken)
        {
            // 1. Fetch Students and their Academic Profile IDs
            var students = await _unitOfWork.StudentRepository
                .GetAll()
                .AsNoTracking()
                .Select(s => new { s.Id, s.UniversityCode, s.AcademicProfileId })
                .ToListAsync(cancellationToken);

            // 2. Fetch all Academic Profiles to get CGPA
            var profiles = await _unitOfWork.StudentAcademicProfileRepository
                .GetAll()
                .AsNoTracking()
                .ToDictionaryAsync(p => p.Id, p => p.CGPA, cancellationToken);

            // 3. Fetch all Enrollments with Courses and their latest Grade
            // To handle "Bulk" efficiently, we fetch all relevant data and group it.
            var enrollments = await _dbContext.Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                .Include(e => e.Grades)
                .ToListAsync(cancellationToken);

            var enrollmentsByStudent = enrollments
                .GroupBy(e => e.StudentId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var result = new List<BatchStudentAIGradesDto>();

            foreach (var s in students)
            {
                var dto = new BatchStudentAIGradesDto
                {
                    UniversityCode = s.UniversityCode,
                    GPA = profiles.GetValueOrDefault(s.AcademicProfileId)
                };

                if (enrollmentsByStudent.TryGetValue(s.Id, out var studentEnrollments))
                {
                    foreach (var e in studentEnrollments)
                    {
                        var latestScore = e.Grades
                            .OrderByDescending(g => g.CreatedAt)
                            .Select(g => g.AbsoluteScore)
                            .FirstOrDefault();

                        dto.CourseGrades[e.Course.Code] = latestScore;
                    }
                }

                result.Add(dto);
            }

            return result;
        }
    }
}
