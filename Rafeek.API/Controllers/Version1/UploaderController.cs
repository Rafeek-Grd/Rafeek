using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Rafeek.API.Routes;
using Rafeek.Application.Handlers.UploaderHandlers.Commands.DownloadFile;
using Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateImage;
using Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateVideo;
using Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadAudio;
using Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadFile;
using Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadImage;
using Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadVideo;
using Rafeek.Application.Localization;

namespace Rafeek.API.Controllers.Version1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class UploaderController : BaseApiController
    {
        private readonly IMediator _mediator;
        public UploaderController(IMediator mediator, IStringLocalizer<Messages> localizer) : base(mediator, localizer)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Upload Image
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route(ApiRoutes.Uploader.UploadImage)]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Upload Video
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route(ApiRoutes.Uploader.UploadVideo)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadVideo([FromForm] UploadVideoCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Upload Audio
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route(ApiRoutes.Uploader.UploadAudio)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadAudio([FromForm] UploadAudioCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route(ApiRoutes.Uploader.UploadFile)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Update Image
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPatch]
        [Route(ApiRoutes.Uploader.UpdateImage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateImage(string name, [FromForm] UpdateImageCommand request)
        {
            request.ImageName = name;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Update Video
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPatch]
        [Route(ApiRoutes.Uploader.UpdateVideo)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVideo(string name ,[FromForm] UpdateVideoCommand request)
        {
            request.VideoName = name;
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Download File
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route(ApiRoutes.Uploader.DownloadFile)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DownloadFile([FromForm] DownloadFileCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Upload List Of Images
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route(ApiRoutes.Uploader.UploadListImage)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadListOfImages([FromForm] UploadMultipleImageCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        /// <summary>
        /// Upload List Of Videos
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost]
        [Route(ApiRoutes.Uploader.UploadListVideo)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadListOfVideos([FromForm] UploadMultipleVideoCommand request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }
    }
}
