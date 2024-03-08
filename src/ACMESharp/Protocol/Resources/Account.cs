using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ACMESharp.Protocol.Resources
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.2
    /// https://datatracker.ietf.org/doc/html/rfc8555#ection-7.3
    /// </summary>
    public class Account
    {
        public string Id { get; set; } //TODO: not in spec

        public object Key { get; set; } //TODO: not in spec

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[] Contact { get; set; }

        public string Status { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? TermsOfServiceAgreed { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
        [Required]        
        public Order[] Orders { get; set; }

        // TODO: are these standard or specific to LE?
        //    "agreement": "https://letsencrypt.org/documents/LE-SA-v1.2-November-15-2017.pdf",
        //    "initialIp": "50.235.30.49",
        //    "createdAt": "2018-05-02T22:23:30Z",
        public string InitialIp { get; set; }
        public string CreatedAt { get; set; }
        public string Agreement { get; set; }
    }
}
