using MediatR;
using Rafeek.Application.Handlers.CourseHandlers.DTOs;
using System;

namespace Rafeek.Application.Handlers.CourseHandlers.Queries.GetCourseById
{
    public class GetCourseByIdQuery : IRequest<CourseDto>
    {
        public Guid Id { get; set; }
    }
}
