using AutoMapper;
using Rafeek.Application.Handlers.AssignmentHandlers.Commands.CreateAssignment;
using Rafeek.Application.Handlers.AssignmentHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Mappings
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<CreateAssignmentCommand, Assignment>();
            CreateMap<Assignment, AssignmentDto>();
            CreateMap<AssignmentSubmission, AssignmentSubmissionDto>();
        }
    }
}
