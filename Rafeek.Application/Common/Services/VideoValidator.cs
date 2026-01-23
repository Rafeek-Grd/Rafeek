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

        private static readonly string[] AllowedExtensions = new[] { ".mp4", ".webm", ".mov", ".avi" };
        private static readonly string[] AllowedContentTypes = new[]
        {
            "video/mp4",
            "video/webm",
            "video/avi",
            "video/quicktime",
            "video/x-msvideo"
        };
        private const long MaxFileSizeBytes = 100L * 1024L * 1024L; // 100 MB
        private const int BufferSize = 81920; // 80 KB

        public VideoValidator
        (
            IWebHostEnvironment env,
            IStringLocalizer<Messages> localizer
        )
        {
            _env = env;
            _localizer = localizer;
        }

        public async Task<(bool Uploaded, string Result)> UploadVideo(IFormFile file, int place = 0, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length <= 0)
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);

            if (!IsValidVideo(file))
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotValid.Value]);

            var uploadPlace = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place));
            if (!Directory.Exists(uploadPlace))
                Directory.CreateDirectory(uploadPlace);

            var uniqueFileName = GetUniqueFileName(file.FileName);
            // Prefix the generated name with the upload place
            uniqueFileName = $"{place}{uniqueFileName}";

            var filePath = Path.Combine(uploadPlace, uniqueFileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, useAsync: true))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                    await stream.FlushAsync(cancellationToken);
                }

                return (true, uniqueFileName);
            }
            catch (Exception ex)
            {
                if (File.Exists(filePath))
                {
                    try { File.Delete(filePath); } catch { }
                }
                return (false, $"Upload failed: {ex.Message}");
            }
        }

        public async Task<bool> DeleteVideo(string fileName, int place)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var safeName = Path.GetFileName(fileName);
            var fullPath = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place), safeName);

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
            var extension = Path.GetExtension(fileName) ?? string.Empty;
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"{timestamp}{extension}";
        }

        public bool IsValidVideo(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = (Path.GetExtension(file.FileName) ?? string.Empty).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return false;

            var contentType = file.ContentType?.ToLowerInvariant() ?? string.Empty;
            if (!AllowedContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
                return false;

            if (file.Length > MaxFileSizeBytes)
                return false;

            return true;
        }

        public bool IsValidVideo(string videoName, string placeHolder)
        {
            if (string.IsNullOrWhiteSpace(videoName))
                return false;

            var extension = (Path.GetExtension(videoName) ?? string.Empty).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return false;

            var fullPath = Path.Combine(_env.WebRootPath, placeHolder, Path.GetFileName(videoName));
            return VideoIsExisted(fullPath);
        }

        public bool VideoIsExisted(string? fullVideoPath)
        {
            if (string.IsNullOrWhiteSpace(fullVideoPath))
                return false;

            var path = fullVideoPath!;
            if (!Path.IsPathRooted(path))
                path = Path.Combine(_env.ContentRootPath, path);

            return System.IO.File.Exists(path);
        }

        public async Task<(bool Uploaded, string Result)> UploadMultipleVideo(List<IFormFile> files, int place)
        {
            if (files == null || files.Count == 0)
                return (true, string.Empty);

            // Limit concurrency for large video files
            var semaphore = new SemaphoreSlim(3);
            var results = new System.Collections.Concurrent.ConcurrentBag<string>();
            var errors = new System.Collections.Concurrent.ConcurrentBag<string>();

            var tasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var result = await UploadVideo(file, place);
                    if (result.Uploaded)
                    {
                        results.Add(result.Result);
                    }
                    else
                    {
                        errors.Add(result.Result);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);

            if (errors.Any())
            {
                return (false, errors.First());
            }

            var concatenated = string.Join(";", results);
            return (true, concatenated);
        }
    }
}