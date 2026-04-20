using System;
using System.Collections.Generic;
using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class AITimetable : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public string OptionName { get; set; } = null!;
        public int MaxLoad { get; set; }
        public int TotalDays { get; set; }
        public string? TimetableName { get; set; }

        public ICollection<AITimetableItem> Items { get; set; } = new HashSet<AITimetableItem>();
    }
}
