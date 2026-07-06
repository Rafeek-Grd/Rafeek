using MediatR;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.DeleteAcademicSchedule
{
    public class DeleteAcademicScheduleCommand: IRequest<string>
    {
        public Guid? LectureId { get; set; }
    }
}
