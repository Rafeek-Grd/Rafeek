using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Rafeek.Application.Localization
{
    public class LocalizationManager
    {
        // use non-generic IStringLocalizer for static access
        private static IStringLocalizer? _staticLocalizer;
        private readonly IStringLocalizer<Messages> _localizer;

        public LocalizationManager(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;
            _staticLocalizer = localizer;
        }

        public string this[string key]
        {
            get
            {
                return _localizer[key];
            }
        }

        public static string GetLocalizedValue(string key)
        {
            return _staticLocalizer?[key] ?? key;
        }

        public static void Configure(IStringLocalizerFactory localizerFactory)
        {
            if (localizerFactory == null) return;

            var baseName = typeof(Messages).FullName ?? typeof(Messages).Name;
            var assemblyName = typeof(Messages).GetTypeInfo().Assembly.GetName().Name ?? string.Empty;

            // Create an IStringLocalizer for the Messages resource
            _staticLocalizer = localizerFactory.Create(baseName, assemblyName);
        }
    }
}