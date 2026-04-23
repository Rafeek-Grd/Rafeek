using MediatR;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;

namespace Rafeek.Application.Handlers.InstructorHandlers.Commands.SubmitSectionGrades
{
    public class SubmitSectionGradesCommand : IRequest<Unit>
    {
        public Guid SectionId { get; set; }
        public List<SectionGradeEntryDto> Grades { get; set; } = new();
    }
}
