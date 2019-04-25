#if !NETSTANDARD
using System.Web.Services.Protocols;

namespace SkyApm.Soap
{
    public class SkyApmSoapHeader : SoapHeader
    {
        public string SkyApmHeaderString { get; set; }
    }
}
#endif