using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.CreateNewCourse
{
    public class CreateNewCourseCommandHandler : IRequestHandler<CreateNewCourseCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateNewCourseCommandHandler(IUnitOfWork ctx, IMapper mapper, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<string> Handle(CreateNewCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Course>(request);
            
            _ctx.CourseRepository.Add(entity);
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0 ?
                _localizer[LocalizationKeys.GlobalValidationMessages.AddedSuccessfully.Value]
                : _localizer[LocalizationKeys.GlobalValidationMessages.AddedFailed.Value];
        }
    }
}
