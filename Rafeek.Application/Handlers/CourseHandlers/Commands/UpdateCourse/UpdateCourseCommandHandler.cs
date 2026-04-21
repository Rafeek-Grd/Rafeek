using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.UpdateCourse
{
    public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateCourseCommandHandler(IUnitOfWork ctx, IMapper mapper, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<string> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.CourseRepository.FindByKeyAsync(request.Id, cancellationToken);

            _mapper.Map(request, entity);
            
            var result = await _ctx.SaveChangesAsync(cancellationToken);

            return result > 0?
                _localizer[LocalizationKeys.GlobalValidationMessages.UpdatedSuccessfully.Value]:
                _localizer[LocalizationKeys.GlobalValidationMessages.UpdatedFailed.Value];
        }
    }
}
