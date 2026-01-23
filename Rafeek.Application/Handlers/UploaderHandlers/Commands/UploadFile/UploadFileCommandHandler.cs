using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadFile
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, string>
    {
        private readonly IFileValidator _fileValidator;

        public UploadFileCommandHandler
        (
            IFileValidator fileValidator
        )
        {
            _fileValidator = fileValidator;
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var result = await _fileValidator.UploadFile(request.File, request.UploadPlace, cancellationToken);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            return result.Result;
        }
    }
}
