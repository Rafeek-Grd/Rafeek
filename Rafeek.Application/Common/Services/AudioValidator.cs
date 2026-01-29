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
        public AudioValidator(IWebHostEnvironment env, IStringLocalizer<Messages> localizer)
        {
            _env = env;
            _localizer = localizer;
        }
        public async Task<(bool Uploaded, string Result)> UploadAudio(IFormFile file, int place = 0)
        {
            string uploadPalce = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place));
            if (file == null || file.Length <= 0)
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);

            if (!IsValidAudio(file))
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

        public async Task<bool> DeleteAudio(string fileName, int place)
        {
            return true;
        }

        public string GetUniqueFileName(string fileName)
        {
            // Generate a unique file name using a combination of timestamp and file extension
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var extension = Path.GetExtension(fileName);
            return $"{timestamp}{extension}";
        }


        public bool IsValidAudio(IFormFile file)
        {
            // Check if the file is not null and has a non-zero length
            if (file == null || file.Length == 0)
                return false;

            // Check the file extension Acc , Ogg , Opus
            var allowedExtensions = new[]
            { ".aac",".AAC", ".ogg",".OGG", ".opus",".OPUS", ".mp3", ".MP3", ".m4a",".M4A",".MIDI",".midi",".AMR",".amr",".WMA",".wma",".WAV",".wav",".WEBM",".webm" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return false;

            // Check the file content type
            var allowedContentTypes = new[] { "audio/mpeg", "audio/mp3", "audio/aac", "audio/ogg", "audio/opus", "audio/wav", "audio/wave", "audio/x-wav", "audio/m4a", "audio/x-m4a", "audio/midi", "audio/x-midi", "audio/amr", "audio/x-ms-wma", "audio/webm" };
            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
                return false;

            // Check the file size (in this example, limit it to 5 MB)
            var maxFileSize = 5 * 1024 * 1024; // 5 MB in bytes
            if (file.Length > maxFileSize)
                return false;

            // You can perform additional checks here, such as image dimensions, aspect ratio, etc.

            return true;
        }

        public bool IsValidAudio(string ImageName, string PlaceHolder)
        {
            // Check the string must end with this extension
            var allowedExtensions = new[]
           { ".aac",".AAC", ".ogg",".OGG", ".opus",".OPUS", ".mp3", ".MP3", ".m4a",".M4A",".MIDI",".midi",".AMR",".amr",".WMA",".wma",".WAV",".wav",".WEBM", ".webm" };

            var Result = allowedExtensions.Any(x => ImageName.EndsWith(x));
            if (Result)
            {
                // check Image are Existed int this Location
                string uploadPalce = Path.Combine(_env.WebRootPath, PlaceHolder);
                string? FullImagePath = Path.Combine(uploadPalce, ImageName);
                return AudioIsExisted(FullImagePath);
            }
            return false;
        }

        public bool AudioIsExisted(string? FullImagePath)
        {
            var image = Path.Combine(Directory.GetCurrentDirectory(), FullImagePath);
            if (System.IO.File.Exists(image))
                return true;
            return false;
        }
    }
}