using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands.AssignStudentsToAcademicAdvisor
{
    public class AssignStudentToAcademicAdvisorCommandHandler : IRequestHandler<AssignStudentsToAcademicAdvisorCommand, Unit>
    {
        private readonly IUnitOfWork _ctx;

        public AssignStudentToAcademicAdvisorCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Unit> Handle(AssignStudentsToAcademicAdvisorCommand request, CancellationToken cancellationToken)
        {
            var students = await _ctx.StudentRepository.GetAll(x => request.StudentIds.Contains(x.UserId)).ToListAsync(cancellationToken);

            foreach (var student in students)
            {
                student.AcademicAdvisorId = request.AcademicAdvisorId;
            }

            _ctx.StudentRepository.UpdateRange(students);
            await _ctx.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
