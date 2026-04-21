using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;

using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseDetail
{
    public class GetCourseDetailQueryHandler : IRequestHandler<GetCourseDetailQuery, CourseDetailDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetCourseDetailQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<CourseDetailDto> Handle(GetCourseDetailQuery request, CancellationToken cancellationToken)
        {
            var courseDetail = await _ctx.CourseRepository.GetAll()
                .AsNoTracking()
                .Where(c => c.Id == request.CourseId)
                .ProjectTo<CourseDetailDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            courseDetail.RegistrationStatus = courseDetail.Capacity == 0 ? "Closed" : (courseDetail.EnrolledStudents >= courseDetail.Capacity ? "Full" : "Open");
            courseDetail.RegistrationStatusLabel = courseDetail.Capacity == 0 ? "إلغاء التسجيل" : (courseDetail.EnrolledStudents >= courseDetail.Capacity ? "مكتمل" : "متاح");

            var prerequisiteIds = courseDetail.Prerequisites.Select(p => p.CourseId).ToList();
            var completedCourseIds = new HashSet<Guid>();
            var inProgressCourseIds = new HashSet<Guid>();

            if (request.StudentId.HasValue && prerequisiteIds.Any())
            {
                var studentEnrollments = await _ctx.EnrollmentRepository.GetAll()
                    .AsNoTracking()
                    .Where(e => e.StudentId == request.StudentId && prerequisiteIds.Contains(e.CourseId))
                    .Select(e => new { e.CourseId, IsCompleted = string.IsNullOrWhiteSpace(e.Grade) == false })
                    .ToListAsync(cancellationToken);

                completedCourseIds = studentEnrollments.Where(e => e.IsCompleted).Select(e => e.CourseId).ToHashSet();
                inProgressCourseIds = studentEnrollments.Where(e => !e.IsCompleted).Select(e => e.CourseId).ToHashSet();
            }

            foreach(var p in courseDetail.Prerequisites)
            {
                p.StudentStatus = "NotMet";
                p.StudentStatusLabel = "غير مكتمل";

                if (request.StudentId.HasValue)
                {
                    if (completedCourseIds.Contains(p.CourseId))
                    {
                        p.StudentStatus = "Met";
                        p.StudentStatusLabel = "مكتمل";
                    }
                    else if (inProgressCourseIds.Contains(p.CourseId))
                    {
                        p.StudentStatus = "InProgress";
                        p.StudentStatusLabel = "جاري";
                    }
                }
            }

            int totalHours = courseDetail.CreditHours > 0 ? courseDetail.CreditHours : 1;
            int uniHours = (int)Math.Round(totalHours * 0.26);
            int majHours = (int)Math.Round(totalHours * 0.43);
            int elective = totalHours - uniHours - majHours;

            courseDetail.StudyPlanDistribution = new StudyPlanDistributionDto
            {
                UniversityRequirementHours = uniHours,
                MajorRequirementHours = majHours,
                ElectiveHours = elective,
                UniversityRequirementPercentage = MathF.Round(uniHours * 100f / totalHours, 1),
                MajorRequirementPercentage = MathF.Round(majHours * 100f / totalHours, 1),
                ElectivePercentage = MathF.Round(elective * 100f / totalHours, 1)
            };

            courseDetail.Notifications = await _ctx.NotificationRepository.GetAll()
                .AsNoTracking()
                .Where(n => n.UserId == null)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .Select(n => new CourseNotificationDto
                {
                    NotificationId = n.Id,
                    Title = n.Title,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return courseDetail;
        }
    }
}
