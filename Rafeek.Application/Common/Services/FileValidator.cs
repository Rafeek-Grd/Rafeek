using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Common.Services
{
    public class FileValidator : IFileValidator
    {
        private readonly IWebHostEnvironment _env;
        private readonly FileExtensionContentTypeProvider _contentProvider;
        private readonly IStringLocalizer<Messages> _localizer;

        private static readonly string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff", ".webp", ".bmp", ".jfif" };
        private static readonly string[] VideoExtensions = new[] { ".mp4", ".webm", ".mov", ".avi" };
        private static readonly string[] AudioExtensions = new[] { ".aac", ".ogg", ".opus", ".mp3", ".m4a", ".midi", ".amr", ".wma", ".wav", ".webm" };
        private static readonly string[] DocumentExtensions = new[] { ".docx", ".doc", ".pdf", ".xls", ".xlsx" };

        private static readonly string[] ImageContentTypes = new[]
        {
            "image/jpeg", "image/jpg", "image/png", "image/gif",
            "image/webp", "image/bmp", "image/x-ms-bmp", "image/jfif",
            "image/pjpeg", "image/x-gif", "application/octet-stream"
        };

        private static readonly string[] VideoContentTypes = new[]
        {
            "video/mp4", "video/webm", "video/avi", "video/quicktime", "video/x-msvideo"
        };

        private static readonly string[] AudioContentTypes = new[]
        {
            "audio/mpeg", "audio/mp3", "audio/aac", "audio/ogg", "audio/opus",
            "audio/wav", "audio/wave", "audio/x-wav", "audio/m4a", "audio/x-m4a",
            "audio/midi", "audio/x-midi", "audio/amr", "audio/x-ms-wma", "audio/webm"
        };

        private const long MaxImageFileSize = 10L * 1024L * 1024L; // 10 MB
        private const long MaxVideoFileSize = 100L * 1024L * 1024L; // 100 MB
        private const long MaxAudioFileSize = 5L * 1024L * 1024L; // 5 MB
        private const long MaxDocumentFileSize = 10L * 1024L * 1024L; // 10 MB (kept previous 5MB relaxed to 10MB for docs)
        private const int BufferSize = 81920; // 80 KB

        public FileValidator
        (
            IWebHostEnvironment env,
            IStringLocalizer<Messages> localizer
        )
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _contentProvider = new FileExtensionContentTypeProvider();
            _localizer = localizer;
        }

        public async Task<(bool Uploaded, string Result)> UploadFile(IFormFile file, int place = 0, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length <= 0)
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);

            if (!IsValidFile(file))
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotValid.Value]);

            var uploadPlace = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place));
            if (!Directory.Exists(uploadPlace))
            {
                Directory.CreateDirectory(uploadPlace);
            }

            var uniqueFileName = GetUniqueFileName(file.FileName);
            // Prefix with the upload place to match Image/Video/Audio validators
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

        public async Task<bool> DeleteFile(string fileName, int place)
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
            var extension = Path.GetExtension(fileName) ?? string.Empty;
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"{timestamp}{extension}";
        }

        public bool IsValidFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var extension = (Path.GetExtension(file.FileName) ?? string.Empty).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            // Determine type by extension
            if (ImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                if (file.Length > MaxImageFileSize)
                    return false;

                var contentType = file.ContentType?.ToLowerInvariant();
                if (!string.IsNullOrWhiteSpace(contentType))
                    return ImageContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase);

                // Try infer from extension
                return _contentProvider.TryGetContentType("dummy" + extension, out var inferred) && !string.IsNullOrWhiteSpace(inferred);
            }

            if (VideoExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                if (file.Length > MaxVideoFileSize)
                    return false;

                var contentType = file.ContentType?.ToLowerInvariant() ?? string.Empty;
                return VideoContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase);
            }

            if (AudioExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                if (file.Length > MaxAudioFileSize)
                    return false;

                var contentType = file.ContentType?.ToLowerInvariant() ?? string.Empty;
                return AudioContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase);
            }

            if (DocumentExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                if (file.Length > MaxDocumentFileSize)
                    return false;

                // For documents don't strictly require content-type match; allow based on extension only
                return true;
            }

            return false;
        }

        public bool IsValidFile(string fileName, string placeHolder)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(placeHolder))
                return false;

            var extension = (Path.GetExtension(fileName) ?? string.Empty).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            var allAllowed = ImageExtensions.Concat(VideoExtensions).Concat(AudioExtensions).Concat(DocumentExtensions);
            if (!allAllowed.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return false;

            var fullFilePath = Path.Combine(_env.WebRootPath, placeHolder, Path.GetFileName(fileName));
            return FileIsExisted(fullFilePath);
        }

        public bool FileIsExisted(string? fullFilePath)
        {
            if (string.IsNullOrWhiteSpace(fullFilePath))
                return false;

            var path = fullFilePath!;
            if (!Path.IsPathRooted(path))
                path = Path.Combine(_env.WebRootPath, path);

            return System.IO.File.Exists(path);
        }

        public async Task<(bool, string)> DownloadFile(int filePlace, string fileName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return (false, string.Empty);

            var fullPath = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(filePlace), Path.GetFileName(fileName));
            if (!FileIsExisted(fullPath))
                return (false, string.Empty);

            try
            {
                // Use stream with buffer instead of reading all bytes at once
                using var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, useAsync: true);
                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream, cancellationToken);
                
                return (true, Convert.ToBase64String(memoryStream.ToArray()));
            }
            catch (Exception)
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);
            }
        }
    }
}