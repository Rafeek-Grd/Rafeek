using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IVideoValidator
    {
        public Task<(bool Uploaded, string Result)> UploadVideo(IFormFile file, int Place);
        public Task<(bool Uploaded, string Result)> UploadMultipleVideo(List<IFormFile> file, int Place);

        public bool VideoIsExisted(string? FullImagePath);
        public Task<bool> DeleteVideo(string fileName, int Place);
        public string GetUniqueFileName(string fileName);
        public bool IsValidVideo(string VideoeName, string PlaceHolder);
        public bool IsValidVideo(IFormFile file);
    }
}
