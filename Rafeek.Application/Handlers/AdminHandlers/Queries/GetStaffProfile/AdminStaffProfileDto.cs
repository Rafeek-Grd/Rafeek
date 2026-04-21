namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStaffProfile
{
    public class AdminStaffProfileDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        
        /// <summary>المسمى الوظيفي: مدرس مساعد، أستاذ مادة، الخ</summary>
        public string Title { get; set; } = null!; 
        
        public string? DepartmentName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        
        // المعلومات الشخصية
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        // ساعات العمل / أوقات المحاضرات
        public List<WorkingHourDto> WorkingHours { get; set; } = new();

        // زملاء المقرر / مساعدو التدريس
        public List<CoTeacherDto> CoTeachers { get; set; } = new();

        // المقررات التي قمت بتدريسها
        public List<StaffCourseDto> TaughtCourses { get; set; } = new();
    }

    public class WorkingHourDto
    {
        public string Days { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Location { get; set; } = null!;
    }

    public class CoTeacherDto
    {
        public string Name { get; set; } = null!;
        public string RoleDesc { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
    }

    public class StaffCourseDto
    {
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string SemesterName { get; set; } = null!;
    }
}
