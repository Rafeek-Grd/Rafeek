using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentAcademicRecords
{
    /// <summary>
    /// بيانات طالب واحد في جدول سجلات الطلاب الأكاديمية بلوحة تحكم الأدمن.
    /// </summary>
    public class StudentAcademicRecordDto
    {
        public Guid StudentId { get; set; }

        /// <summary>الاسم الكامل للطالب</summary>
        public string FullName { get; set; } = null!;

        /// <summary>البريد الجامعي</summary>
        public string UniversityEmail { get; set; } = null!;

        /// <summary>الرقم الجامعي</summary>
        public string UniversityCode { get; set; } = null!;

        /// <summary>اسم القسم</summary>
        public string? DepartmentName { get; set; }

        /// <summary>المعدل التراكمي</summary>
        public float Cgpa { get; set; }

        /// <summary>
        /// الحالة الأكاديمية — القيم: "Stable" | "Warning" | "Probation"
        /// </summary>
        public string AcademicStatus { get; set; } = null!;

        /// <summary>التسمية العربية للحالة (منتظم / تحذير / إنذار أول)</summary>
        public string AcademicStatusLabel { get; set; } = null!;

        public int Level { get; set; }
        public int Term { get; set; }
    }
}
