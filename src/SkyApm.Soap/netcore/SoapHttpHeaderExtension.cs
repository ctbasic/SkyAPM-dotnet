#if NETSTANDARD
using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Net.Http;

namespace SkyApm.Soap.netcore
{
    public static class SoapHttpHeaderExtension
    {
        const string SOAP_ACTION_TAG = "SOAPAction";

        /// <summary>
        /// 是否soap请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsSoapRequest(this HttpRequestMessage request)
        {
            if (request.Headers.Contains(SOAP_ACTION_TAG))
            {
                return true;
            }
            if (request.Content.Headers.ContentType.MediaType.ToLower().Contains("soap"))
            {
                return true;
            }
            return false;
        }

        public static string GetSoapActionName(this HttpRequestMessage request)
        {
            if (!request.IsSoapRequest())
            {
                return string.Empty;
            }

            if (request.Headers != null)
            {
                foreach (var header in request.Headers)
                {
                    if (header.Key == SOAP_ACTION_TAG)
                    {
                        var actionUri = new Uri(header.Value.FirstOrDefault().Replace("\"", string.Empty));
                        string actionName = actionUri.PathAndQuery.Replace("/", "");

                        return actionName;
                    }
                }
            }
            var xDocument = XDocument.Load(request.Content.ReadAsStreamAsync().Result);
            var soapBody = xDocument.Descendants().FirstOrDefault(o => o.Name.LocalName.Equals("Body", StringComparison.CurrentCultureIgnoreCase));
            return ((System.Xml.Linq.XElement)soapBody.FirstNode).Name.LocalName;
        }
    }
}
#endif