using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AdminHandlers.Queries;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorDashboard
{
    public class GetInstructorDashboardQueryHandler : IRequestHandler<GetInstructorDashboardQuery, InstructorDashboardDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetInstructorDashboardQueryHandler(IUnitOfWork ctx, IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<InstructorDashboardDto> Handle(GetInstructorDashboardQuery request, CancellationToken cancellationToken)
        {
            var instructorUserId = _currentUserService.UserId;

            // ── 1. Statistics ────────────────────────────
            var sectionIds = await _ctx.LectureGroupRepository
                .IncludeAll(null)
                .Where(x => x.Doctor.UserId == instructorUserId)
                .AsNoTracking()
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);

            var totalStudents = await _ctx.EnrollmentRepository
                .GetAll(e => sectionIds.Contains(e.LectureGroupId))
                .Select(e => e.StudentId)
                .Distinct()
                .CountAsync(cancellationToken);

            var pendingToGrade = await _ctx.AssignmentSubmissionRepository
                .IncludeAll(null)
                .Where(s => sectionIds.Contains(s.Assignment.LectureGroupId) && s.Score == null)
                .CountAsync(cancellationToken);

            // ── 2. Get Student Academic Records for this Doctor ────────────────────────────
            // Step 1: Resolve Doctor from current user
            var doctor = await _context.Doctors
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.UserId == instructorUserId, cancellationToken);

            PagginatedResult<StudentAcademicRecordDto> studentAcademicRecords;

            if (doctor == null)
            {
                studentAcademicRecords = new PagginatedResult<StudentAcademicRecordDto>(
                    new List<StudentAcademicRecordDto>().AsReadOnly(),
                    0,
                    request.PageNumber,
                    request.PageSize);
            }
            else
            {
                // Step 2: Get all LectureGroup IDs belonging to this Doctor
                var doctorLectureGroupIds = await _context.LectureGroups
                    .AsNoTracking()
                    .Where(lg => lg.DoctorId == doctor.Id)
                    .Select(lg => lg.Id)
                    .ToListAsync(cancellationToken);

                // Step 3: If LectureGroupId filter is provided, validate it belongs to this doctor
                bool skipRecords = false;
                if (request.LectureGroupId.HasValue)
                {
                    if (!doctorLectureGroupIds.Contains(request.LectureGroupId.Value))
                    {
                        skipRecords = true;
                    }
                    else
                    {
                        doctorLectureGroupIds = new List<Guid> { request.LectureGroupId.Value };
                    }
                }

                if (skipRecords || !doctorLectureGroupIds.Any())
                {
                    studentAcademicRecords = new PagginatedResult<StudentAcademicRecordDto>(
                        new List<StudentAcademicRecordDto>().AsReadOnly(),
                        0,
                        request.PageNumber,
                        request.PageSize);
                }
                else
                {
                    // Step 4: Get distinct StudentIds enrolled in those LectureGroups
                    var studentIds = await _context.Enrollments
                        .AsNoTracking()
                        .Where(e => doctorLectureGroupIds.Contains(e.LectureGroupId))
                        .Select(e => e.StudentId)
                        .Distinct()
                        .ToListAsync(cancellationToken);

                    if (!studentIds.Any())
                    {
                        studentAcademicRecords = new PagginatedResult<StudentAcademicRecordDto>(
                            new List<StudentAcademicRecordDto>().AsReadOnly(),
                            0,
                            request.PageNumber,
                            request.PageSize);
                    }
                    else
                    {
                        // Step 5: Query Students filtered to those IDs
                        var recordsQuery = _context.Students
                            .AsNoTracking()
                            .Where(s => studentIds.Contains(s.Id))
                            .Include(s => s.User)
                            .Include(s => s.Department)
                            .Include(s => s.AcademicProfile)
                            .AsQueryable();

                        // ── Filtering ────────────────────────────────────────────────────────
                        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                        {
                            var term = request.SearchTerm.Trim();
                            recordsQuery = recordsQuery.Where(s =>
                                s.User.FullName.Contains(term) ||
                                s.UniversityCode.Contains(term) ||
                                (s.User.Email != null && s.User.Email.Contains(term)));
                        }

                        if (!string.IsNullOrWhiteSpace(request.AcademicStatus))
                        {
                            recordsQuery = recordsQuery.Where(s => s.AcademicProfile != null && s.AcademicProfile.Standing == request.AcademicStatus);
                        }

                        if (request.Cgpa.HasValue)
                        {
                            recordsQuery = recordsQuery.Where(s => s.AcademicProfile != null && s.AcademicProfile.CGPA == request.Cgpa.Value);
                        }

                        var totalRecordsCount = await recordsQuery.CountAsync(cancellationToken);

                        var recordItems = await recordsQuery
                            .OrderBy(s => s.User.FullName)
                            .Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .Select(s => new StudentAcademicRecordDto
                            {
                                StudentId = s.Id,
                                FullName = s.User.FullName,
                                UniversityEmail = s.User.Email!,
                                UniversityCode = s.UniversityCode,
                                DepartmentName = s.Department != null ? s.Department.Name : null,
                                Cgpa = s.AcademicProfile != null ? s.AcademicProfile.CGPA : 0f,
                                AcademicStatus = s.AcademicProfile != null ? s.AcademicProfile.Standing : "Stable",
                                AcademicStatusLabel = s.AcademicProfile == null ? "منتظم"
                                                    : s.AcademicProfile.Standing == "Stable" ? "منتظم"
                                                    : s.AcademicProfile.Standing == "Warning" ? "تحذير"
                                                    : s.AcademicProfile.Standing == "Probation" ? "إنذار أول"
                                                    : s.AcademicProfile.Standing,
                                Level = s.Level,
                                Term = s.Term
                            })
                            .ToListAsync(cancellationToken);

                        studentAcademicRecords = new PagginatedResult<StudentAcademicRecordDto>(
                            recordItems.AsReadOnly(),
                            totalRecordsCount,
                            request.PageNumber,
                            request.PageSize);
                    }
                }
            }

            // ── 3. Assemble ────────────────────────────
            return new InstructorDashboardDto
            {
                TotalSections = sectionIds.Count,
                TotalStudents = totalStudents,
                PendingAssignmentsToGrade = pendingToGrade,
                StudentAcademicRecords = studentAcademicRecords
            };
        }
    }
}
