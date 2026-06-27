using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetStudentProfileForInstructor
{
    public class GetStudentProfileForInstructorQueryHandler : IRequestHandler<GetStudentProfileForInstructorQuery, InstructorStudentProfileDto>
    {
        private readonly IUnitOfWork _ctx;

        public GetStudentProfileForInstructorQueryHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<InstructorStudentProfileDto> Handle(GetStudentProfileForInstructorQuery request, CancellationToken cancellationToken)
        {
            var student = await _ctx.StudentRepository
                .GetFirstIncludingAll(x => x.Id == request.StudentId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return new InstructorStudentProfileDto
            {
                StudentId = student!.Id,
                FullName = student.User.FullName,
                UniversityCode = student.UniversityCode,
                ProfileImageUrl = student.User.ProfilePictureUrl,
                Major = student.Department?.Name,
                CGPA = student.AcademicProfile?.CGPA
            };
        }
    }
}
