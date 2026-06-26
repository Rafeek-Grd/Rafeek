using MediatR;

namespace Rafeek.Application.Handlers.GenericHandlers.GetProfilesForAdmins
{
    public class GetProfilesForAdminsQuery: IRequest<GetProfilesForAdminsResponse>
    {
        public Guid? StaffId { get; set; }
        public Guid? StudentId { get; set; }
        public Guid? MentorId { get; set; }
        public Guid? ProfessorId { get; set; }
    }
}


