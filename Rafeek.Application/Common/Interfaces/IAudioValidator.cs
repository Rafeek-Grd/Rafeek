using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IAudioValidator
    {
        public Task<(bool Uploaded, string Result)> UploadAudio(IFormFile file, int Place, CancellationToken cancellationToken = default);
        public bool AudioIsExisted(string? FullImagePath);
        public Task<bool> DeleteAudio(string fileName, int Place);
        public string GetUniqueFileName(string fileName);
        public bool IsValidAudio(string VideoeName, string PlaceHolder);
        public bool IsValidAudio(IFormFile file);
    }
}
