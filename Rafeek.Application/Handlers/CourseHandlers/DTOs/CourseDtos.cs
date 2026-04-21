namespace Rafeek.Application.Handlers.CourseHandlers.DTOs
{
    // ─── List DTO (الشاشة الأولى - بطاقة المقرر في القائمة) ─────────────────────
    public class CourseListItemDto
    {
        public Guid CourseId { get; set; }

        /// <summary>رمز المقرر مثل CS-102</summary>
        public string Code { get; set; } = null!;

        public string Title { get; set; } = null!;
        public int CreditHours { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? DepartmentId { get; set; }

        /// <summary>اسم مدرس المادة الرئيسي</summary>
        public string? InstructorName { get; set; }

        /// <summary>عدد الطلاب المسجلين حالياً</summary>
        public int EnrolledStudents { get; set; }

        /// <summary>الطاقة الاستيعابية القصوى (يُحدَّد من Section.Capacity)</summary>
        public int Capacity { get; set; }

        /// <summary>
        /// حالة المقرر — القيم: "Open" | "Full" | "Closed"
        /// الواجهة تعرضها: متاح / مكتمل / إلغاء التسجيل
        /// </summary>
        public string Status { get; set; } = null!;
        public string StatusLabel { get; set; } = null!;

        /// <summary>المتطلبات الأساسية (رموز المقررات فقط للبطاقة)</summary>
        public List<string> PrerequisiteCodes { get; set; } = new();

        /// <summary>اسم الفصل الدراسي المربوط (مثال: Fall 2024)</summary>
        public string? AcademicTerm { get; set; }
    }

    // ─── Detail DTO (الشاشة الثانية - تفاصيل مقرر واحد) ────────────────────────
    public class CourseDetailDto
    {
        public Guid CourseId { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CreditHours { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? DepartmentId { get; set; }

        // ── حالة التسجيل ──────────────────────────────────────────────────────
        /// <summary>"Open" | "Full" | "Closed"</summary>
        public string RegistrationStatus { get; set; } = null!;
        public string RegistrationStatusLabel { get; set; } = null!;
        public int EnrolledStudents { get; set; }
        public int Capacity { get; set; }

        /// <summary>تاريخ بداية المقرر</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>تاريخ فتح التسجيل</summary>
        public DateTime? RegistrationOpenDate { get; set; }

        /// <summary>الفصل الدراسي المربوط بالمقرر</summary>
        public string? AcademicTerm { get; set; }

        // ── نوع المقرر (تاج نظري / تطبيقي) ────────────────────────────────────
        public bool IsTheoretical { get; set; }
        public bool IsPractical { get; set; }

        // ── المستوى الأكاديمي المستهدف ─────────────────────────────────────────
        public int TargetLevel { get; set; }

        // ── المتطلبات الأساسية ─────────────────────────────────────────────────
        public List<PrerequisiteStatusDto> Prerequisites { get; set; } = new();

        // ── الخطة الدراسية (توزيع الساعات) ────────────────────────────────────
        public StudyPlanDistributionDto StudyPlanDistribution { get; set; } = new();

        // ── مدرسو المادة ───────────────────────────────────────────────────────
        public List<CourseInstructorDto> Instructors { get; set; } = new();

        // ── إشعارات خاصة بالمقرر ──────────────────────────────────────────────
        public List<CourseNotificationDto> Notifications { get; set; } = new();
    }

    public class PrerequisiteStatusDto
    {
        public Guid CourseId { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;

        /// <summary>
        /// حالة المتطلب للطالب — القيم: "Met" | "InProgress" | "NotMet"
        /// الواجهة تعرضها: أخضر / أصفر / أحمر
        /// </summary>
        public string StudentStatus { get; set; } = "NotMet";
        public string StudentStatusLabel { get; set; } = "غير مكتمل";
    }

    public class StudyPlanDistributionDto
    {
        /// <summary>ساعات المتطلبات الجامعية</summary>
        public int UniversityRequirementHours { get; set; }

        /// <summary>ساعات المتطلبات التخصصية</summary>
        public int MajorRequirementHours { get; set; }

        /// <summary>ساعات المقررات الاختيارية</summary>
        public int ElectiveHours { get; set; }

        public float UniversityRequirementPercentage { get; set; }
        public float MajorRequirementPercentage { get; set; }
        public float ElectivePercentage { get; set; }
    }

    public class CourseInstructorDto
    {
        public Guid InstructorId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }

        /// <summary>ساعات المكتب</summary>
        public string? OfficeHours { get; set; }
    }

    public class CourseNotificationDto
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
