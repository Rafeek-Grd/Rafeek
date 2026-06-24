using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAllStudentsGradesBatch
{
    public class GetAllStudentsGradesBatchQueryHandler : IRequestHandler<GetAllStudentsGradesBatchQuery, PagginatedResult<BatchStudentAIGradesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRafeekDbContext _dbContext;

        public GetAllStudentsGradesBatchQueryHandler(IUnitOfWork unitOfWork, IRafeekDbContext dbContext)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public async Task<PagginatedResult<BatchStudentAIGradesDto>> Handle(GetAllStudentsGradesBatchQuery request, CancellationToken cancellationToken)
        {
            var students = await _unitOfWork.StudentRepository
                .GetAll()
                .AsNoTracking()
                .Select(s => new { s.Id, s.UniversityCode, s.AcademicProfileId })
                .ToListAsync(cancellationToken);

            var profiles = await _unitOfWork.StudentAcademicProfileRepository
                .GetAll()
                .AsNoTracking()
                .ToDictionaryAsync(p => p.Id, p => p.CGPA, cancellationToken);

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
                    StudentId = s.Id.ToString(),
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

            var totalCount = result.Count;
            List<BatchStudentAIGradesDto> items;

            if (request.PageNumber == -1)
            {
                items = result;
            }
            else
            {
                items = result
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();
            }

            return new PagginatedResult<BatchStudentAIGradesDto>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
