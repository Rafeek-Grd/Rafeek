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
        private readonly IStringLocalizer<Messages> _localizer;

        public FileValidator(IWebHostEnvironment env, IStringLocalizer<Messages> localizer)
        {
            _env = env;
            _localizer = localizer;
        }
        public async Task<(bool Uploaded, string Result)> UploadFile(IFormFile file, int place = 0)
        {
            string uploadPalce = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(place));
            if (file == null || file.Length <= 0)
                return (false, _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);

            if (!IsValidFile(file))
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



        public async Task<bool> DeleteFile(string fileName, int place)
        {
            //if (string.IsNullOrEmpty(fileName))
            //    return false;
            //string Location = Enum.GetName(typeof(UploadPlace), Place);
            //string? FullImagePath = $"{_environment.WebRootPath}\\{Location}\\{fileName}";

            //if (!await ImageIsExisted(FullImagePath))
            //    return false;

            //var image = Path.Combine(Directory.GetCurrentDirectory(), FullImagePath);
            //System.IO.File.Delete(image);
            return true;
        }

        public string GetUniqueFileName(string fileName)
        {
            // Generate a unique file name using a combination of timestamp and file extension
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            var extension = Path.GetExtension(fileName);
            return $"{timestamp}{extension}";
        }


        public bool IsValidFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff",
             ".docx", ".doc", ".pdf",".xlx",".xlsx"};
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return false;


            // Check the file size (in this example, limit it to 5 MB)
            var maxFileSize = 5 * 1024 * 1024; // 5 MB in bytes
            if (file.Length > maxFileSize)
                return false;


            return true;
        }

        public bool IsValidFile(string FileName, string PlaceHolder)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".tiff",
             ".docx", ".doc", ".pdf",".xlx",".xlsx"};

            var Result = allowedExtensions.Any(x => FileName.EndsWith(x));
            if (Result)
            {
                string uploadPalce = Path.Combine(_env.WebRootPath, PlaceHolder);
                string? FullFilePath = Path.Combine(uploadPalce, FileName);
                return FileIsExisted(FullFilePath);
            }
            return false;
        }

        public bool FileIsExisted(string? FullImagePath)
        {
            var image = Path.Combine(Directory.GetCurrentDirectory(), FullImagePath);
            if (System.IO.File.Exists(image))
                return true;
            return false;
        }

        public async Task<(bool, string)> DownloadFile(int FilePlace, string FileName)
        {

            string FilePlath = Path.Combine(_env.WebRootPath, UploadPaths.GetPath(FilePlace));
            var _GetFilePath = $"{FilePlath}{FileName}";
            if (!FileIsExisted(_GetFilePath))
            {
                return (false, string.Empty);
            }
            try
            {
                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(_GetFilePath, out var _ContentType))
                {
                    _ContentType = "application/octet-stream";
                }
                var _ReadAllBytesAsync = await File.ReadAllBytesAsync(_GetFilePath);
                return (true, Convert.ToBase64String(_ReadAllBytesAsync));
            }
            catch (Exception ex)
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);
            }
        }
    }
}