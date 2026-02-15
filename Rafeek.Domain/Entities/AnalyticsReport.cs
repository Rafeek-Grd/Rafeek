using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class AnalyticsReport : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        
        public ReportType ReportType { get; set; }
        
        public DateTime GeneratedAt { get; set; }
        
        public string FileUrl { get; set; } = null!;
    }
}
