#if !NETSTANDARD
using System;
using System.Web.Services.Protocols;
#endif
using System.Linq;

namespace SkyApm.Soap
{
    internal static class SoapHeaderUtils
    {
        /// <summary>
        ///     soap中头信息键名
        /// </summary>
        public const string SOAP_HEADER_NAME = "SkyApmSoapHeader";

        /// <summary>
        ///     头信息不同键值对信息分隔符
        /// </summary>
        private const char HEADER_SEPARATOR = ';';

        /// <summary>
        ///     头信息键值对分隔符
        /// </summary>
        private const char HEADER_KEY_VALUE_SEPARATOR = '|';

#if !NETSTANDARD
        public static SoapICarrierHeader GetSoapICarrierHeader(SoapHeaderCollection soapHeaders)
        {
            try
            {
                SoapICarrierHeader carrierHeader = new SoapICarrierHeader();
                if (soapHeaders == null || soapHeaders.Count <= 0)
                {
                    return carrierHeader;
                }

                var header = soapHeaders[0];
                if (!(header is SoapUnknownHeader))
                {
                    return carrierHeader;
                }

                var unknownHeader = header as SoapUnknownHeader;
                if (unknownHeader.Element.Name != SOAP_HEADER_NAME)
                {
                    return carrierHeader;
                }

                string headersValue = unknownHeader.Element.InnerText;

                string[] headers = headersValue.Split(HEADER_SEPARATOR);

                foreach (var keyValueStr in headers)
                {
                    string[] keyValue = keyValueStr.Split(HEADER_KEY_VALUE_SEPARATOR);
                    string key = "";
                    string value = "";
                    if (keyValue.Length == 2)
                    {
                        key = keyValue[0];
                        value = keyValue[1];
                    }

                    if (!string.IsNullOrWhiteSpace(key) && !carrierHeader.Contains(key))
                    {
                        carrierHeader.Add(key, value);
                    }
                }

                return carrierHeader;
            }
            catch (Exception e)
            {
                throw new Exception("GetSoapICarrierHeader error", e);
            }
        }
#endif

        /// <summary>
        ///     包装头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string WrapSoapICarrierHeader(SoapICarrierHeader headers)
        {
            if (headers == null || headers.ToList().Count <= 0)
            {
                return string.Empty;
            }

            string headerStr = string.Empty;
            foreach (var keyValue in headers.ToList())
            {
                headerStr += $"{keyValue.Key}{HEADER_KEY_VALUE_SEPARATOR}{keyValue.Value}{HEADER_SEPARATOR}";
            }

            return headerStr.TrimEnd(HEADER_SEPARATOR);
        }
    }
}