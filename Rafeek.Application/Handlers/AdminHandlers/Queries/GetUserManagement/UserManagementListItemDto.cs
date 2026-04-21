using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetUserManagement
{
    public class UserManagementListItemDto
    {
        /// <summary>معرّف المستخدم (ApplicationUser ID) يُستخدم في الإجراءات</summary>
        public Guid UserId { get; set; }

        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

        /// <summary>
        /// دور المستخدم. القيم الممكنة:
        /// "Student", "Instructor", "Doctor", "Advisor", "Staff", "Admin"
        /// </summary>
        public string Role { get; set; } = null!;

        public string? DepartmentName { get; set; }

        /// <summary>
        /// حالة الحساب — القيم: "Active" | "Inactive"
        /// يعتمد على هل الإيميل الجامعي مفعل، أو حالة الطالب نشط/غير نشط
        /// </summary>
        public string Status { get; set; } = null!;
        public string StatusLabel { get; set; } = null!;

        /// <summary>الرقم الجامعي للطالب، أو الرقم الوظيفي لباقي الأدوار</summary>
        public string IdentificationNumber { get; set; } = null!;
    }
}
