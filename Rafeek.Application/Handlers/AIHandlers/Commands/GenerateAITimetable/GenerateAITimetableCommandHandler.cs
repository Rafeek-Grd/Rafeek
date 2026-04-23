using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.GenerateAITimetable
{
    public class GenerateAITimetableCommandHandler : IRequestHandler<GenerateAITimetableCommand, AITimetableResponseDto>
    {
        private readonly IAIService _aiService;
        private readonly IMemoryCache _cache;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<Messages> _localizer;

        public GenerateAITimetableCommandHandler(
            IAIService aiService, 
            IMemoryCache cache, 
            ICurrentUserService currentUserService,
            IStringLocalizer<Messages> localizer)
        {
            _aiService = aiService;
            _cache = cache;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<AITimetableResponseDto> Handle(GenerateAITimetableCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var cacheKey = $"RateLimit_GenerateTimetable_{userId}";

            if (_cache.TryGetValue(cacheKey, out int requestCount))
            {
                if (requestCount >= 3)
                {
                    throw new BadRequestException(_localizer["RateLimit_Exceeded"]);
                }
            }

            var result = await _aiService.GenerateScheduleAsync(request.TimetableRequest, cancellationToken);
            
            if (result != null)
            {
                var newCount = requestCount + 1;
                _cache.Set(cacheKey, newCount, TimeSpan.FromHours(1));
            }

            return result!;
        }
    }
}
