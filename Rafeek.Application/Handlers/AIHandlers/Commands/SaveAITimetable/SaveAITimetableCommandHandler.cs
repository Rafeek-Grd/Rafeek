using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.SaveAITimetable
{
    public class SaveAITimetableCommandHandler : IRequestHandler<SaveAITimetableCommand, Guid>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Messages> _localizer;

        public SaveAITimetableCommandHandler(IUnitOfWork ctx, IMapper mapper, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Guid> Handle(SaveAITimetableCommand request, CancellationToken cancellationToken)
        {
            // Verify student exists
            var studentExists = await _ctx.StudentRepository.ExistsByKeyAsync(request.StudentId, cancellationToken);
            if (!studentExists)
                throw new NotFoundException(nameof(Student), request.StudentId);

            AITimetable? timetable = null;

            if (request.Id.HasValue)
            {
                timetable = await _ctx.AITimetableRepository.GetAll()
                    .Include(x => x.Items)
                    .FirstOrDefaultAsync(x => x.Id == request.Id.Value, cancellationToken);

                if (timetable != null)
                {
                    // Map basic properties from Stats
                    _mapper.Map(request.TimetableData, timetable);
                    timetable.TimetableName = request.TimetableName ?? timetable.TimetableName;
                    timetable.IsActive = true;

                    // Clear existing items to replacement
                    if (timetable.Items.Any())
                    {
                        _ctx.AITimetableItemRepository.DeleteRange(timetable.Items);
                        timetable.Items.Clear();
                    }

                    // Map new items
                    var newItems = _mapper.Map<List<AITimetableItem>>(request.TimetableData.Schedule);
                    foreach (var item in newItems)
                    {
                        item.TimetableId = timetable.Id;
                        timetable.Items.Add(item);
                    }
                }
            }

            if (timetable == null)
            {
                timetable = _mapper.Map<AITimetable>(request.TimetableData);
                timetable.StudentId = request.StudentId;
                timetable.TimetableName = request.TimetableName;
                timetable.IsActive = true;
                
                _ctx.AITimetableRepository.Add(timetable);
            }

            await _ctx.SaveChangesAsync(cancellationToken);

            return timetable.Id;
        }
    }
}
