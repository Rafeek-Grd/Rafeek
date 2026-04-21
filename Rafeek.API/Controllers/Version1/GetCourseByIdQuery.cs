using MediatR;

namespace Rafeek.API.Controllers.Version1
{
    internal class GetCourseByIdQuery : IRequest<string>
    {
        public Guid Id { get; set; }
    }
}