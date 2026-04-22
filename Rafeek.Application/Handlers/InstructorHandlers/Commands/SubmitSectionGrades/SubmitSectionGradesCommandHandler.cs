using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Commands.SubmitSectionGrades
{
    public class SubmitSectionGradesCommandHandler : IRequestHandler<SubmitSectionGradesCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public SubmitSectionGradesCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(SubmitSectionGradesCommand request, CancellationToken cancellationToken)
        {
            var enrollmentIds = request.Grades.Select(g => g.EnrollmentId).ToList();

            var enrollments = await _ctx.EnrollmentRepository
                .GetAll(e => enrollmentIds.Contains(e.Id) && e.SectionId == request.SectionId)
                .ToListAsync(cancellationToken);

            if (enrollments.Count == 0)
                throw new NotFoundException("Section", request.SectionId);

            using var transaction = await _ctx.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var gradeEntry in request.Grades)
                {
                    var enrollment = enrollments.FirstOrDefault(e => e.Id == gradeEntry.EnrollmentId);
                    if (enrollment == null) continue;

                    var grade = new Grade
                    {
                        EnrollmentId = enrollment.Id,
                        AbsoluteScore = gradeEntry.Score,
                        GradeValue = gradeEntry.Score,
                        TermGPA = gradeEntry.TermGPA,
                        CGPA = gradeEntry.CGPA
                    };
                    _ctx.EnrollmentRepository.Add(enrollment);
                    enrollment.Grades.Add(grade);
                }

                await _ctx.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            return Unit.Value;
        }
    }
}
