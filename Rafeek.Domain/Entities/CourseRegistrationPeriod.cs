using Rafeek.Domain.Common;
using System;

namespace Rafeek.Domain.Entities
{
    public class CourseRegistrationPeriod : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Guid AcademicTermId { get; set; }
        public AcademicTerm AcademicTerm { get; set; } = null!;

        public DateTime RegistrationOpeningDate { get; set; }
        public DateTime RegistrationClosingDate { get; set; }
    }
}
