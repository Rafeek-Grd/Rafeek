namespace Rafeek.Application.Common.Extensions
{
    public static class ToGuidExtension
    {
        // Extension method to convert nullable Guid to Guid
        public static Guid ToGuid(this Guid? nullableGuid)
        {
            return nullableGuid.ToGuidOrDefault(Guid.Empty);
        }

        // Overloaded extension method to provide a default value if nullableGuid is null
        public static Guid ToGuidOrDefault(this Guid? nullableGuid, Guid defaultValue)
        {
            return nullableGuid.HasValue ? nullableGuid.Value : defaultValue;
        }


        public static Guid ToGuid(this string? str)
        {
            Guid guid;
            if (string.IsNullOrEmpty(str) || !Guid.TryParse(str, out guid))
            {
                guid = Guid.Empty; // Assign a default Guid value
            }

            return guid;
        }

        public static Guid? ToGuid(this object? obj)
        {
            Guid guid;
            if (obj == null || !Guid.TryParse(obj?.ToString(), out guid))
            {
                guid = Guid.Empty; // Assign a default Guid value
            }

            return new Guid(guid.ToString());
        }

        public static bool IsGuid(this string? input)
        {
            return Guid.TryParse(input, out _);
        }
    }
}
