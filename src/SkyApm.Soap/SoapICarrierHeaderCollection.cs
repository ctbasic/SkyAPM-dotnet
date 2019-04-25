using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using SkyApm.Tracing;

namespace SkyApm.Soap
{
    internal class SoapICarrierHeaderCollection : ICarrierHeaderCollection
    {
        private readonly SoapICarrierHeader soapICarrierHeader;

        public SoapICarrierHeaderCollection(SoapICarrierHeader headers)
        {
            soapICarrierHeader = headers;
        }

        public void Add(string key, string value)
        {
            soapICarrierHeader.Add(key, value);
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return soapICarrierHeader.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}