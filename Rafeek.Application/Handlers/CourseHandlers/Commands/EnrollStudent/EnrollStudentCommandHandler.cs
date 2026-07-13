using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.EnrollStudent
{
    public class EnrollStudentCommandHandler : IRequestHandler<EnrollStudentCommand, bool>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;

        public EnrollStudentCommandHandler(IUnitOfWork ctx, ICurrentUserService currentUserService, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<bool> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _ctx.StudentRepository
                .GetFirstAsync(s => s.UserId == _currentUserService.UserId, cancellationToken);

            if (student == null) throw new UnauthorizedException(_localizer[LocalizationKeys.Student.StudentProfileNotFound]);

            var existingEnrollment = await _ctx.EnrollmentRepository
                .ExistsAsync(e => e.StudentId == student.Id && e.CourseId == request.CourseId, cancellationToken);

            if (existingEnrollment) throw new BadRequestException(_localizer[LocalizationKeys.Course.AlreadyEnrolled]);

            var lectureGroup = await _ctx.LectureGroupRepository
                .ExistsAsync(s => s.Id == request.LectureGroupId && s.CourseId == request.CourseId, cancellationToken);

            if (!lectureGroup) throw new NotFoundException(_localizer[LocalizationKeys.ExceptionMessage.NotFound]);

            var section = await _ctx.CourseSectionRepository
                .ExistsAsync(s => s.Id == request.SectionId && s.CourseId == request.CourseId, cancellationToken);

            if (!section) throw new NotFoundException(_localizer[LocalizationKeys.Course.SectionNotFound]);

            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CourseId = request.CourseId,
                LectureGroupId = request.LectureGroupId,
                SectionId = request.SectionId,
                Status = "Active"
            };

            await _ctx.EnrollmentRepository.AddAsync(enrollment, cancellationToken);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
