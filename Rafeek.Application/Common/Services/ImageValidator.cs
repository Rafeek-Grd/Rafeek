using System.Text;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;
using Rafeek.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Rafeek.Application.Localization;
using Microsoft.Extensions.Localization;

namespace Rafeek.Application.Common.Services
{
    public class ImageValidator : IImageValidator
    {
        private readonly IWebHostEnvironment _env;
        private readonly FileExtensionContentTypeProvider _contentProvider;
        private readonly IStringLocalizer<Messages> _localizer;

        private static readonly string[] AllowedExtensions = new[] { ".jpg", ".jpeg", ".webp", ".bmp", ".png", ".gif", ".jfif" };
        private static readonly string[] AllowedContentTypes = new[]
        {
            "image/jpeg", "image/jpg", "image/png", "image/gif",
            "image/webp", "image/bmp", "image/x-ms-bmp", "image/jfif",
            "image/pjpeg", "image/x-gif", "application/octet-stream"
        };

        private const long MaxImageFileSize = 10L * 1024L * 1024L; // 10 MB
        private const int BufferSize = 81920; // 80 KB buffer for optimal streaming

        public ImageValidator
        (
            IWebHostEnvironment env, 
            FileExtensionContentTypeProvider contentProvider,
            IStringLocalizer<Messages> localizer
        )
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _contentProvider = contentProvider ?? new FileExtensionContentTypeProvider();
            _localizer = localizer;
        }

        public async Task<(bool Uploaded, string Result)> UploadImage(IFormFile file, int place = 0, CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length <= 0)
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);

            if (string.IsNullOrWhiteSpace(file.FileName))
                return (false, _localizer[LocalizationKeys.UploadFileMessages.NameRequried.Value]);

            if (!IsValidImage(file))
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotValid.Value]);

            var uploadPlace = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place));
            if (!Directory.Exists(uploadPlace))
                Directory.CreateDirectory(uploadPlace);

            var uniqueFileName = GetUniqueFileName(file.FileName);
            // Prefix the generated name with the upload place (first number)
            uniqueFileName = $"{place}{uniqueFileName}";

            var filePath = Path.Combine(uploadPlace, uniqueFileName);

            try
            {
                // Validate GIF signature if needed, but do it efficiently
                var extension = Path.GetExtension(uniqueFileName)?.ToLowerInvariant();
                if (extension == ".gif")
                {
                    using (var readStream = file.OpenReadStream())
                    {
                        if (!await IsValidGifFile(readStream))
                        {
                            return (false, "Invalid GIF file format");
                        }
                    }
                }

                // Use FileOptions.SequentialScan and optimal buffer size for performance
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, FileOptions.Asynchronous | FileOptions.SequentialScan))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                    // FlushAsync is generally not needed after CopyToAsync unless specifically required by logic
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

        private static async Task<bool> IsValidGifFile(Stream stream)
        {
            if (stream == null || stream.Length < 6)
                return false;

            try
            {
                if (stream.CanSeek)
                    stream.Position = 0;

                byte[] header = new byte[6];
                var read = await stream.ReadAsync(header, 0, header.Length);
                
                // Reset position if possible for subsequent reads
                if (stream.CanSeek)
                    stream.Position = 0;

                if (read < header.Length)
                    return false;

                var signature = Encoding.ASCII.GetString(header);
                return signature == "GIF89a" || signature == "GIF87a";
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteImage(string fileName, int place)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var safeName = Path.GetFileName(fileName);
            var filePath = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place), safeName);

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    await Task.CompletedTask;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetUniqueFileName(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension))
                extension = ".jpg";

            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"{timestamp}{extension}";
        }

        public bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0 || file.Length > MaxImageFileSize)
                return false;

            if (string.IsNullOrWhiteSpace(file.FileName))
                return false;

            var extension = (Path.GetExtension(file.FileName) ?? string.Empty).ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return false;

            var contentType = file.ContentType?.ToLowerInvariant();
            if (!string.IsNullOrWhiteSpace(contentType))
            {
                if (!AllowedContentTypes.Contains(contentType, StringComparer.OrdinalIgnoreCase))
                    return false;
            }
            else
            {
                // If content type missing, attempt to infer from extension
                if (!_contentProvider.TryGetContentType("dummy" + extension, out var inferred) || string.IsNullOrWhiteSpace(inferred))
                    return false;
            }

            return true;
        }

        public bool IsValidImage(string imageName, string placeHolder)
        {
            if (string.IsNullOrWhiteSpace(imageName) || string.IsNullOrWhiteSpace(placeHolder))
                return false;

            var extension = (Path.GetExtension(imageName) ?? string.Empty).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                return false;

            var fullImagePath = Path.Combine(_env.WebRootPath, placeHolder, Path.GetFileName(imageName));
            return ImageIsExisted(fullImagePath);
        }

        public bool ImageIsExisted(string? fullImagePath)
        {
            if (string.IsNullOrWhiteSpace(fullImagePath))
                return false;

            var path = fullImagePath!;
            if (!Path.IsPathRooted(path))
                path = Path.Combine(_env.WebRootPath, path);

            return File.Exists(path);
        }

        public async Task<(bool Uploaded, string Result)> UploadMultipleImage(System.Collections.Generic.List<IFormFile> files, int place)
        {
            if (files == null || !files.Any())
                return (false, "No files provided");

            // Use SemaphoreSlim to limit concurrency if needed (e.g. max 5 parallel uploads)
            var semaphore = new SemaphoreSlim(5);
            var results = new System.Collections.Concurrent.ConcurrentBag<string>();
            var errors = new System.Collections.Concurrent.ConcurrentBag<string>();

            var tasks = files.Where(f => f != null).Select(async file =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var result = await UploadImage(file, place);
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

            var resultString = string.Join(";", results);
            return (true, resultString);
        }

        public async Task<IFormFile> ConvertImageToFormFile(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                throw new ArgumentException(nameof(imageUrl));

            using var client = new HttpClient();
            var response = await client.GetAsync(imageUrl);
            response.EnsureSuccessStatusCode();

            var memoryStream = new MemoryStream();
            await (await response.Content.ReadAsStreamAsync()).CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var fileName = Path.GetFileName(new Uri(imageUrl).AbsolutePath);
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(extension))
            {
                var contentTypeHeader = response.Content.Headers.ContentType?.MediaType?.ToLowerInvariant();
                extension = contentTypeHeader switch
                {
                    "image/jpeg" or "image/jpg" => ".jpg",
                    "image/png" => ".png",
                    "image/gif" => ".gif",
                    "image/webp" => ".webp",
                    "image/bmp" or "image/x-ms-bmp" => ".bmp",
                    "image/jfif" => ".jfif",
                    _ => ".jpg"
                };

                fileName = $"{Path.GetFileNameWithoutExtension(fileName)}{extension}";
            }

            var contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                ".bmp" => "image/bmp",
                ".jfif" => "image/jfif",
                _ => "application/octet-stream"
            };

            var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return formFile;
        }
    }
}