using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Localization
{
    public class KeyStringJsonConverter : JsonConverter<KeyString>
    {
        public override KeyString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read as string and create KeyString
            var value = reader.GetString();
            return value != null ? new KeyString(value) : null;
        }

        public override void Write(Utf8JsonWriter writer, KeyString value, JsonSerializerOptions options)
        {
            // Write the localized value instead of the key
            writer.WriteStringValue(value?.Value ?? string.Empty);
        }
    }
}
