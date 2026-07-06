using MediatR;
using Rafeek.Application.Handlers.AcademicSchedules.DTOs;

namespace Rafeek.Application.Handlers.AcademicSchedules.Queries.GetAcademicSchedulesById
{
    public class GetAcademicSchedulesByIdQuery : IRequest<AcademicScheduleDto>
    {
        public Guid LectureId { get; set; }
    }
}
