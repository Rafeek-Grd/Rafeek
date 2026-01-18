using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.Extensions.Primitives;
using Rafeek.Application.Common.Services;

namespace Rafeek.API.Services
{
    public class CustomFileProvider : IFileProvider
    {
        private readonly string _wwwRootPath;

        public CustomFileProvider(string? wwwRootPath = null)
        {
            _wwwRootPath = !string.IsNullOrWhiteSpace(wwwRootPath)
                ? wwwRootPath
                : Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var filePath = Path.GetFileName(subpath) ?? string.Empty;
            if (string.IsNullOrEmpty(filePath))
            {
                return new NotFoundFileInfo(subpath);
            }

            // Try first digit as place
            if (filePath.Length >= 1 && char.IsDigit(filePath[0]))
            {
                if (int.TryParse(filePath.Substring(0, 1), out var place1))
                {
                    var fileLocation1 = Path.Combine(_wwwRootPath, UploadPaths.GetPath(place1), filePath);
                    if (File.Exists(fileLocation1))
                        return new PhysicalFileInfo(new FileInfo(fileLocation1));
                }
            }

            // Fallback: try first two digits (keeps previous behaviour)
            if (filePath.Length >= 2 && char.IsDigit(filePath[0]) && char.IsDigit(filePath[1]))
            {
                if (int.TryParse(filePath.Substring(0, 2), out var place2))
                {
                    var fileLocation2 = Path.Combine(_wwwRootPath, UploadPaths.GetPath(place2), filePath);
                    if (File.Exists(fileLocation2))
                        return new PhysicalFileInfo(new FileInfo(fileLocation2));
                }
            }

            // Not found in upload paths: return NotFound so other static middlewares can handle (or 404)
            return new NotFoundFileInfo(filePath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return NotFoundDirectoryContents.Singleton;
        }

        public IChangeToken Watch(string filter)
        {
            return NullChangeToken.Singleton;
        }
    }
}