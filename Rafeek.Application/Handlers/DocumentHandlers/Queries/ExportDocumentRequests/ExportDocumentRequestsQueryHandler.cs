using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.DocumentHandlers.Queries.ExportDocumentRequests
{
    public class ExportDocumentRequestsQueryHandler : IRequestHandler<ExportDocumentRequestsQuery, byte[]>
    {
        private readonly IUnitOfWork _ctx;

        public ExportDocumentRequestsQueryHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<byte[]> Handle(ExportDocumentRequestsQuery request, CancellationToken cancellationToken)
        {
            var query = _ctx.DocumentRequestRepository
                .IncludeAll(null)
                .AsNoTracking();

            // Apply filters
            if (request.Status.HasValue)
            {
                query = query.Where(x => x.Status == request.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.DocumentType))
            {
                query = query.Where(x => x.DocumentType == request.DocumentType);
            }

            if (request.StudentId.HasValue)
            {
                query = query.Where(x => x.StudentId == request.StudentId.Value);
            }

            if (request.AdvisorId.HasValue)
            {
                query = query.Where(x => x.Student.AcademicAdvisorId == request.AdvisorId.Value);
            }

            if (request.DepartmentId.HasValue)
            {
                query = query.Where(x => x.Student.DepartmentId == request.DepartmentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();
                query = query.Where(x => x.Student.User.FullName.Contains(term) || x.Student.UniversityCode.Contains(term));
            }

            var list = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new
                {
                    StudentName = x.Student.User.FullName,
                    UniversityCode = x.Student.UniversityCode,
                    RequestType = x.DocumentType,
                    Topic = x.Topic,
                    Date = x.CreatedAt,
                    Status = x.Status,
                    Remarks = x.Remarks
                })
                .ToListAsync(cancellationToken);

            var sb = new StringBuilder();
            // CSV Header with UTF-8 BOM
            sb.AppendLine("الطالب,الرقم الجامعي,نوع الطلب,المقرر / الموضوع,تاريخ التقديم,الحالة,الملاحظات");

            foreach (var item in list)
            {
                var statusLabel = item.Status == Rafeek.Domain.Enums.DocumentStatus.Pending ? "قيد الانتظار" :
                                  item.Status == Rafeek.Domain.Enums.DocumentStatus.Approved ? "مقبول" : "مرفوض";

                var dateStr = item.Date.ToString("yyyy-MM-dd HH:mm");
                var studentName = EscapeCsvValue(item.StudentName);
                var universityCode = EscapeCsvValue(item.UniversityCode);
                var requestType = EscapeCsvValue(item.RequestType);
                var topic = EscapeCsvValue(item.Topic ?? "");
                var remarks = EscapeCsvValue(item.Remarks ?? "");

                sb.AppendLine($"{studentName},{universityCode},{requestType},{topic},{dateStr},{statusLabel},{remarks}");
            }

            var csvBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var bom = new byte[] { 0xEF, 0xBB, 0xBF };
            var result = new byte[bom.Length + csvBytes.Length];
            Buffer.BlockCopy(bom, 0, result, 0, bom.Length);
            Buffer.BlockCopy(csvBytes, 0, result, bom.Length, csvBytes.Length);

            return result;
        }

        private string EscapeCsvValue(string value)
        {
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\r") || value.Contains("\n"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }
            return value;
        }
    }
}
