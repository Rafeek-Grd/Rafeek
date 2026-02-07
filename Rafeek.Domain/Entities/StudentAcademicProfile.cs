using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class StudentAcademicProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; } = null!;

        public float GPA { get; set; }

        public float CGPA { get; set; }

        public int CompletedCredits { get; set; }

        public int RemainingCredits { get; set; }

        [MaxLength(50)]
        public string Standing { get; set; } = null!;

      
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public StudentAcademicProfile()
        {
            IsActive = true;
            IsDeleted = false;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
