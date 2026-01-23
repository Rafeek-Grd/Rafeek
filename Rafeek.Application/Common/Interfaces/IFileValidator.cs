using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IFileValidator
    {
        public Task<(bool Uploaded, string Result)> UploadFile(IFormFile file, int Place, CancellationToken cancellationToken = default);
        public bool FileIsExisted(string? FullFilePath);
        public Task<bool> DeleteFile(string fileName, int Place);
        public string GetUniqueFileName(string fileName);
        public bool IsValidFile(string FileName, string PlaceHolder);
        public bool IsValidFile(IFormFile file);
        public Task<(bool, string)> DownloadFile(int FilePlace, string FileName, CancellationToken cancellationToken = default);
    }
}
