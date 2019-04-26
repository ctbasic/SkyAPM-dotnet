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
        public static SoapICarrierHeaders ExtractSoapICarrierHeader(this SoapHeaderCollection soapHeaders)
        {
            try
            {
                SoapICarrierHeaders carrierHeaders = new SoapICarrierHeaders();
                if (soapHeaders == null || soapHeaders.Count <= 0)
                {
                    return carrierHeaders;
                }

                var header = soapHeaders[0];
                if (!(header is SoapUnknownHeader))
                {
                    return carrierHeaders;
                }

                var unknownHeader = header as SoapUnknownHeader;
                if (unknownHeader.Element.Name != SOAP_HEADER_NAME)
                {
                    return carrierHeaders;
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

                    if (!string.IsNullOrWhiteSpace(key) && !carrierHeaders.Contains(key))
                    {
                        carrierHeaders.Add(key, value);
                    }
                }

                return carrierHeaders;
            }
            catch (Exception e)
            {
                throw new Exception("ExtractSoapICarrierHeader error", e);
            }
        }
#endif

        /// <summary>
        ///     包装头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string EncodeSoapICarrierHeader(this SoapICarrierHeaders headers)
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

        /// <summary>
        ///     包装头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static SoapICarrierHeaders ExtractSoapICarrierHeader(this string headers)
        {
            SoapICarrierHeaders carrierHeaders = new SoapICarrierHeaders();
            if (string.IsNullOrWhiteSpace(headers))
            {
                return carrierHeaders;
            }

            string[] headerKeyValues = headers.Split(HEADER_SEPARATOR);

            foreach (var keyValueStr in headerKeyValues)
            {
                string[] keyValue = keyValueStr.Split(HEADER_KEY_VALUE_SEPARATOR);
                string key = "";
                string value = "";
                if (keyValue.Length == 2)
                {
                    key = keyValue[0];
                    value = keyValue[1];
                }

                if (!string.IsNullOrWhiteSpace(key) && !carrierHeaders.Contains(key))
                {
                    carrierHeaders.Add(key, value);
                }
            }

            return carrierHeaders;
        }
    }
}