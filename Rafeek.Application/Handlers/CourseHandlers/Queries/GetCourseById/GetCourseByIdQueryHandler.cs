using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseById
{
    public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDto>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IMapper _mapper;

        public GetCourseByIdQueryHandler(IUnitOfWork ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<CourseDto> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.CourseRepository.GetAll()
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            return _mapper.Map<CourseDto>(entity);
        }
    }
}
