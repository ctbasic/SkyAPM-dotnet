#if !NETSTANDARD
using System;
using System.Web;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SkyApm.Soap.netframework
{
    public static class SoapHttpHeaderExtension
    {
        const string SOAP_ACTION_NAME = "SOAP_ACTION_NAME";

        /// <summary>
        /// 注入soap头到http 请求头
        /// </summary>
        /// <param name="request"></param>
        public static void InjectSoapHeaderToHttpHeader(this HttpRequest request)
        {
            if (!request.IsSoapRequest())
            {
                return;
            }
            using (Stream inputStream = request.GetBufferedInputStream())
            {

                var xDocument = XDocument.Load(inputStream);
                var soapBody = xDocument.Descendants().FirstOrDefault(o => o.Name.LocalName.Equals("Body", StringComparison.CurrentCultureIgnoreCase));
                var soapAction = ((System.Xml.Linq.XElement)soapBody.FirstNode).Name.LocalName;
                request.RequestContext.HttpContext.Items[SOAP_ACTION_NAME] = soapAction;

                var skyApmHeader = xDocument.Descendants().FirstOrDefault(o => o.Name.LocalName.Equals("SkyApmHeaderString", StringComparison.CurrentCultureIgnoreCase));
                if (skyApmHeader == null || string.IsNullOrWhiteSpace(skyApmHeader.Value))
                {
                    return;
                }

                SoapICarrierHeaders headers = skyApmHeader.Value.ExtractSoapICarrierHeader();
                if (headers == null || !headers.Any())
                {
                    return;
                }
                foreach (var item in headers)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// 是否soap请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsSoapRequest(this HttpRequest request)
        {
            if (request.ServerVariables["HTTP_SOAPACTION"] != null)
            {
                return true;
            }
            if (request.ContentType.Contains("soap"))
            {
                return true;
            }
            return false;
        }

        public static string GetSoapActionName(this HttpRequest request)
        {
            var item = request.RequestContext.HttpContext.Items[SOAP_ACTION_NAME];
            if (item == null)
            {
                return string.Empty;
            }
            return item.ToString();
        }
    }
}
#endif