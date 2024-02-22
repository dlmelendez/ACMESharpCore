using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ACMESharp.Protocol.Resources
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.3
    /// </summary>
    public class Order
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public string Status { get; set; }

        public string Expires { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required, MinLength(1)]
        public Identifier[] Identifiers { get; set; }

        public string NotBefore { get; set; }

        public string NotAfter { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Problem? Error { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required, MinLength(1)]
        public string[] Authorizations { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public string Finalize { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Certificate { get; set; }

    }
}
