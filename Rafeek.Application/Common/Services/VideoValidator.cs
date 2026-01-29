using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Common.Services
{
    public class VideoValidator : IVideoValidator
    {
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<Messages> _localizer;

        public VideoValidator(IWebHostEnvironment env, IStringLocalizer<Messages> localizer)
        {
            _env = env;
            _localizer = localizer;
        }
        public async Task<(bool Uploaded, string Result)> UploadVideo(IFormFile file, int place = 0)
        {
            string uploadPalce = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place));
            if (file == null || file.Length <= 0)
                return (false, "FileNotFound");

            if (!IsValidVideo(file))
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotValid.Value]);

            var uniqueFileName = $"{place}{GetUniqueFileName(file.FileName)}";
            var filePath = Path.Combine(uploadPalce, uniqueFileName);
            // check folder is exist 
            if (!Directory.Exists(uploadPalce))
            {
                Directory.CreateDirectory(uploadPalce);
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return (true, uniqueFileName);
        }

        public async Task<bool> DeleteVideo(string fileName, int place)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var fullPath = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place), Path.GetFileName(fileName));
            if (!File.Exists(fullPath))
                return false;

            try
            {
                File.Delete(fullPath);
                await Task.CompletedTask;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string GetUniqueFileName(string fileName)
        {
            // Generate a unique file name using a combination of timestamp and file extension
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var extension = Path.GetExtension(fileName);
            return $"{timestamp}{extension}";
        }
        public bool IsValidVideo(IFormFile file)
        {
            // Check if the file is not null and has a non-zero length
            if (file == null || file.Length == 0)
                return false;

            // Check the file extension
            var allowedExtensions = new[] { ".mp4", ".webm", ".mov", ".avi" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return false;

            // Check the file content type
            var allowedContentTypes = new[]
            {
                "video/mp4",
                "video/webm",
                "video/avi",
                "video/quicktime",
                "video/x-msvideo"
            };
            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
                return false;

            // Check the file size (in this example, limit it to 100 MB)
            var maxFileSize = 100 * 1024 * 1024; // 100 MB in bytes
            if (file.Length > maxFileSize)
                return false;

            // You can perform additional checks here, such as image dimensions, aspect ratio, etc.

            return true;
        }

        public bool IsValidVideo(string VideoeName, string PlaceHolder)
        {
            // Check the string must end with this extension
            var allowedExtensions = new[] { ".mp4", ".webm", ".mov", ".avi" };

            var Result = allowedExtensions.Any(x => VideoeName.EndsWith(x));
            if (Result)
            {
                // check Image are Existed int this Location
                string uploadPalce = Path.Combine(_env.WebRootPath, PlaceHolder);
                string? FullImagePath = $"{uploadPalce}\\{VideoeName}";
                return VideoIsExisted(FullImagePath);
            }
            return false;
        }

        public bool VideoIsExisted(string? FullImagePath)
        {
            var image = Path.Combine(Directory.GetCurrentDirectory(), FullImagePath);
            if (System.IO.File.Exists(image))
                return true;
            return false;
        }

        public async Task<(bool Uploaded, string Result)> UploadMultipleVideo(List<IFormFile> file, int Place)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in file)
            {
                var result = UploadVideo(item, Place);
                if (!result.Result.Uploaded)
                {
                    return (false, result.Result.Result);
                }
                stringBuilder.Append(result.Result.Result + ";");
            }
            return (true, stringBuilder.ToString());
        }
    }
}