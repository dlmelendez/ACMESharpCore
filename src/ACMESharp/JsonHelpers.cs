using System.Text.Json;

namespace ACMESharp
{
    public static class JsonHelpers
    {
        public static readonly JsonSerializerOptions JsonWebOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        public static readonly JsonSerializerOptions JsonWebIndentedOptions = new JsonSerializerOptions(JsonWebOptions) { WriteIndented = true };

    }
}
