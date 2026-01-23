using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Common.Services
{
    public class AudioValidator : IAudioValidator
    {
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<Messages> _localizer;

        private static readonly string[] AllowedExtensions = new[]
        {
            ".aac", ".ogg", ".opus", ".mp3", ".m4a", ".midi", ".amr", ".wma", ".wav", ".webm"
        };

        private static readonly string[] AllowedContentTypes = new[]
        {
            "audio/mpeg", "audio/mp3", "audio/aac", "audio/ogg", "audio/opus",
            "audio/wav", "audio/wave", "audio/x-wav", "audio/m4a", "audio/x-m4a",
            "audio/midi", "audio/x-midi", "audio/amr", "audio/x-ms-wma", "audio/webm"
        };

        private const long MaxFileSizeBytes = 5L * 1024L * 1024L; // 5 MB
        private const int BufferSize = 81920; // 80 KB

        public AudioValidator
        (
            IWebHostEnvironment env,
            IStringLocalizer<Messages> localizer
        )
        {
            _env = env;
            _localizer = localizer;
        }

        public async Task<(bool Uploaded, string Result)> UploadAudio(IFormFile file, int place = 0, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length <= 0)
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);

            if (!IsValidAudio(file))
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotValid.Value]);

            var uploadPlace = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place));
            if (!Directory.Exists(uploadPlace))
            {
                Directory.CreateDirectory(uploadPlace);
            }

            var uniqueFileName = GetUniqueFileName(file.FileName);
            // Prefix the generated name with the upload place to match ImageValidator and VideoValidator behavior
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

        public async Task<bool> DeleteAudio(string fileName, int place)
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

        public bool IsValidAudio(IFormFile file)
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

        public bool IsValidAudio(string fileName, string placeHolder)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var extension = (Path.GetExtension(fileName) ?? string.Empty).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return false;

            var fullPath = Path.Combine(_env.WebRootPath, placeHolder, Path.GetFileName(fileName));
            return AudioIsExisted(fullPath);
        }

        public bool AudioIsExisted(string? fullAudioPath)
        {
            if (string.IsNullOrWhiteSpace(fullAudioPath))
                return false;

            var path = fullAudioPath!;
            if (!Path.IsPathRooted(path))
                path = Path.Combine(_env.WebRootPath, path);

            return System.IO.File.Exists(path);
        }
    }
}