using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands
{
    public class RequestGuidanceCommand : IRequest<Guid>
    {
        public Guid StudentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class RequestGuidanceCommandHandler : IRequestHandler<RequestGuidanceCommand, Guid>
    {
        private readonly IRafeekDbContext _context;

        public RequestGuidanceCommandHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(RequestGuidanceCommand request, CancellationToken cancellationToken)
        {
            var studentExists = await _context.Students.AnyAsync(x => x.Id == request.StudentId, cancellationToken);
            if (!studentExists)
            {
                throw new Exception("Student not found.");
            }

            var supportRequest = new StudentSupport
            {
                StudentId = request.StudentId,
                Title = request.Title,
                Description = request.Description,
                StudentSupportStatus = StudentSupportStatus.Pending
            };

            _context.StudentSupports.Add(supportRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return supportRequest.Id;
        }
    }
}
