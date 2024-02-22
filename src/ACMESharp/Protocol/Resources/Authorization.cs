using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ACMESharp.Protocol.Resources
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.4
    /// </summary>
    public class Authorization
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public Identifier Identifier { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public string Status { get; set; } = string.Empty;

        public string Expires { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public Challenge[] Challenges { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Wildcard { get; set; }
    }
}
