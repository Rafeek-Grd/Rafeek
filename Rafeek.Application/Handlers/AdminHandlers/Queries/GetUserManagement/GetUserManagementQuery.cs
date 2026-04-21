using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetUserManagement
{
    public class GetUserManagementQuery : IRequest<PagginatedResult<UserManagementListItemDto>>
    {
        /// <summary>
        /// نوع الحساب المراد جلبه. (مطلوب لتمثيل الـ Tabs بالشاشة)
        /// القيمة الافتراضية: Student
        /// أمثلة: 32 = الطلاب (Student)، 8 = طاقم التدريس (Instructor)، 16 = المرشدين (Doctor)، 4 = (Staff)
        /// </summary>
        public UserType TabRole { get; set; } = UserType.Student;

        /// <summary>
        /// البحث بالاسم أو الإيميل أو الرقم (الجامعي/الوظيفي)
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// حالة الحساب: "Active" | "Inactive"
        /// إن تُرك فارغاً سيجلب الكل
        /// </summary>
        public string? Status { get; set; }

        /// <summary>فلتر بالقسم الأكاديمي (إن وُجد)</summary>
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// (خاص بتبويب المرشدين الأكاديميين)
        /// أرسله true لجلب المرشدين فقط، أو false لجلب أعضاء هيئة التدريس العاديين.
        /// (يعمل هذا الفلتر عندما يكون TabRole = 16 यानी Doctor)
        /// </summary>
        public bool? IsAdvisor { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
