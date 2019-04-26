using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using SkyApm.Tracing;

namespace SkyApm.Soap
{
    internal class SoapICarrierHeaderCollection : ICarrierHeaderCollection
    {
        private readonly SoapICarrierHeaders soapICarrierHeaders;

        public SoapICarrierHeaderCollection(SoapICarrierHeaders headerses)
        {
            soapICarrierHeaders = headerses;
        }

        public void Add(string key, string value)
        {
            soapICarrierHeaders.Add(key, value);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return soapICarrierHeaders.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}