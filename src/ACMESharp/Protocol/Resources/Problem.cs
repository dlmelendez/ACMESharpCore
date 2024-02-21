using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ACMESharp.Protocol.Resources
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/rfc8555#section-6.7
    /// </summary>
    public class Problem
    {
        public const string StandardProblemTypeNamespace = "urn:ietf:params:acme:error:";

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]
        public string Type { get; set; }

        public string Detail { get; set; }

        public int? Status { get; set; }

        /// <summary>
        /// https://datatracker.ietf.org/doc/html/rfc8555#section-6.7.1
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Problem[]? Subproblems { get; set; }
    }
}
