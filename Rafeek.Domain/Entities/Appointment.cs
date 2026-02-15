using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
        public AppointmentStatus Status { get; set; }
    }
}
