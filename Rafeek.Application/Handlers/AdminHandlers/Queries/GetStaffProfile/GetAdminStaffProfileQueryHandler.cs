using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStaffProfile
{
    public class GetAdminStaffProfileQueryHandler : IRequestHandler<GetAdminStaffProfileQuery, AdminStaffProfileDto>
    {
        private readonly IRafeekDbContext _context;

        public GetAdminStaffProfileQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<AdminStaffProfileDto> Handle(GetAdminStaffProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;

            var instructor = await _context.Instructors
                .AsNoTracking()
                .Include(i => i.User)
                .Include(i => i.Department)
                .FirstOrDefaultAsync(i => i.UserId == userId, cancellationToken);

            var doctor = await _context.Doctors
                .AsNoTracking()
                .Include(d => d.User)
                .Include(d => d.Department)
                .FirstOrDefaultAsync(d => d.UserId == userId, cancellationToken);

            var staff = await _context.Staffs
                .AsNoTracking()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

            var user = instructor?.User ?? doctor?.User ?? staff?.User;

            if (user == null)
            {
                throw new NotFoundException($"المستخدم بالمعرّف {userId} غير موجود.");
            }

            var dto = new AdminStaffProfileDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber ?? "غير متوفر",
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            if (doctor != null)
            {
                dto.Title = doctor.IsAcademicAdvisor ? "أستاذ / مرشد أكاديمي" : "أستاذ دكتور";
                dto.DepartmentName = doctor.Department?.Name ?? "عام";

                if (instructor != null)
                {
                    var sections = await _context.Sections
                        .AsNoTracking()
                        .Include(s => s.Course)
                        .Include(s => s.CalendarEvents)
                            .ThenInclude(ce => ce.AcademicTerm)
                                .ThenInclude(at => at!.AcademicYear)
                        .Where(s => s.InstructorId == instructor.Id)
                        .ToListAsync(cancellationToken);

                    ProcessSections(sections, dto, doctor.User.FullName);
                }
            }
            else if (instructor != null)
            {
                dto.Title = "مدرس مساعد";
                dto.DepartmentName = instructor.Department?.Name ?? "عام";

                var sections = await _context.Sections
                    .AsNoTracking()
                    .Include(s => s.Course)
                    .Include(s => s.CalendarEvents)
                        .ThenInclude(ce => ce.AcademicTerm)
                            .ThenInclude(at => at!.AcademicYear)
                    .Where(s => s.InstructorId == instructor.Id)
                    .ToListAsync(cancellationToken);

                ProcessSections(sections, dto, instructor.User.FullName);
            }
            else if (staff != null)
            {
                dto.Title = "موظف إداري";
                dto.DepartmentName = "-";
            }
            else
            {
                dto.Title = "موظف عام";
                dto.DepartmentName = "-";
            }

            return dto;
        }

        private void ProcessSections(List<Domain.Entities.Section> sections, AdminStaffProfileDto dto, string staffName)
        {
            var coursesSet = new HashSet<Guid>();

            foreach (var section in sections)
            {
                string location = "قاعة محاضرات (عامة)";
                var eventTerm = section.CalendarEvents?.FirstOrDefault()?.AcademicTerm;
                
                string semesterName = "-";
                if (eventTerm != null)
                {
                    semesterName = $"{eventTerm.Name} {eventTerm.AcademicYear?.Name}";
                }

                if (!dto.WorkingHours.Any(w => w.Days == section.Day && w.Time == section.Time))
                {
                    dto.WorkingHours.Add(new WorkingHourDto
                    {
                        Days = section.Day ?? "أيام غير محددة",
                        Time = section.Time ?? "وقت غير محدد",
                    
                        Location = "قاعة محاضرات (1)"
                    });
                }

                if (coursesSet.Add(section.CourseId))
                {
                    dto.TaughtCourses.Add(new StaffCourseDto
                    {
                        CourseCode = section.Course.Code,
                        CourseTitle = section.Course.Title,
                        SemesterName = semesterName
                    });
                }
            }


        }
    }
}
