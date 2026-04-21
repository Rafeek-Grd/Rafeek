using MediatR;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentAcademicRecords
{
    public class GetStudentAcademicRecordsQuery : IRequest<PagginatedResult<StudentAcademicRecordDto>>
    {
        /// <summary>
        /// فلتر الحالة الأكاديمية — القيم: "Stable" | "Warning" | "Probation"
        /// اتركه فارغاً لإرجاع جميع الطلاب.
        /// </summary>
        public string? AcademicStatus { get; set; }

        /// <summary>بحث بالاسم أو الرقم الجامعي أو الإيميل</summary>
        public string? SearchTerm { get; set; }

        /// <summary>فلتر بمعرّف القسم</summary>
        public Guid? DepartmentId { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
