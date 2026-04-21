using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.ReminderHandlers.DTOs;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ReminderHandlers.Queries.GetRemindersPaginated
{
    public class GetRemindersPaginatedQueryHandler 
        : IRequestHandler<GetRemindersPaginatedQuery, PagginatedResult<ReminderDto>>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetRemindersPaginatedQueryHandler(IUnitOfWork ctx, IMapper mapper, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagginatedResult<ReminderDto>> Handle(
            GetRemindersPaginatedQuery request, 
            CancellationToken cancellationToken)
        {
            var query = _ctx.ReminderRepository.GetAll()
                .Where(x => x.UserId == _currentUserService.UserId);

            if (!string.IsNullOrEmpty(request.SearchTerm))
                query = query.Where(x => x.Title.Contains(request.SearchTerm) || (x.Description != null && x.Description.Contains(request.SearchTerm)));

            if (request.IsCompleted.HasValue)
                query = query.Where(x => x.IsCompleted == request.IsCompleted.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var entities = await query
                .OrderBy(x => x.DueDate)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<ReminderDto>>(entities);

            return PagginatedResult<ReminderDto>.Create(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
