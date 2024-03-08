using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACMESharp.Protocol.Resources
{
    /// <summary>
    /// https://datatracker.ietf.org/doc/html/rfc8555#section-7.1.2.1
    /// </summary>
    public class OrderList
    {
        public string[] Orders { get; set; }
    }
}
