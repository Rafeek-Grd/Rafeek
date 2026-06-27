using MediatR;
using System;

namespace Rafeek.Application.Handlers.StaffHandlers.GetStaffDashboard
{
    public class GetStaffDashboardQuery : IRequest<GetStaffDashboardDto>
    {
        public string? AcademicStatus { get; set; }
        public string? SearchTerm { get; set; }
        public float? Cgpa { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
