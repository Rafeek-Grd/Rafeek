using Microsoft.Extensions.Configuration;
using Rafeek.Application.Common.Options;

namespace Rafeek.Application.Common.Services
{
    public static class UploadPaths
    {
        private static UploadPathsOptions? Options;

        public static void Configure(IConfiguration configuration)
        {
            Options = configuration.GetSection("UploadPaths").Get<UploadPathsOptions>();
        }

        public static string? RootPath => Options?.Root;

        public static string? UserImagePath => Options?.UserImages;

        public static string? GeneralImagePath => Options?.GeneralImages;

        public static string? GeneralVideoPath => Options?.GeneralVideos;

        public static string? GeneralAudioPath => Options?.GeneralAudios;

        public static string? GeneralFilePath => Options?.GeneralFiles;

        public static string GetPath(int place)
        {
            switch (place)
            {
                case 0:
                    return Options?.Root ?? string.Empty;
                case 1:
                    return Options?.UserImages ?? string.Empty;
                case 2:
                    return Options?.GeneralImages ?? string.Empty;
                case 3:
                    return Options?.GeneralVideos ?? string.Empty;
                case 4:
                    return Options?.GeneralAudios ?? string.Empty;
                case 5:
                    return Options?.GeneralFiles ?? string.Empty;
                default:
                    return Options?.Root ?? string.Empty;
            }
        }
    }
}