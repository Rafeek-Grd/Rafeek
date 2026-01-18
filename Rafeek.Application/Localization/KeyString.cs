using System.Text.Json.Serialization;

namespace Rafeek.Application.Localization
{
    [JsonConverter(typeof(KeyStringJsonConverter))]
    public class KeyString
    {
        private readonly string key;

        public KeyString(string value)
        {
            this.key = value;
        }

        public string Value
        {
            get
            {
                return LocalizationManager.GetLocalizedValue(this.key);
            }
        }

        public string GetLocalizedValue()
        {
            return this.Value;
        }

        public string Key
        {
            get
            {
                return this.key;
            }
        }

        // Implicit conversion to string
        public static implicit operator string(KeyString keyString)
        {
            return keyString?.Value ?? string.Empty;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
