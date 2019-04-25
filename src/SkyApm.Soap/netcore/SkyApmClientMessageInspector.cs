#if NETSTANDARD
using CommonServiceLocator;
using SkyApm.Tracing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using SkyApm.Common;
using SkyApm.Tracing.Segments;

namespace SkyApm.Soap.netcore
{
    internal class SkyApmClientMessageInspector : IClientMessageInspector
    {
        private readonly ITracingContext tracingContext;
        private readonly IExitSegmentContextAccessor segmentContextAccessor;

        public SkyApmClientMessageInspector()
        {
            tracingContext = ServiceLocator.Current.GetInstance<ITracingContext>();
            segmentContextAccessor = ServiceLocator.Current.GetInstance<IExitSegmentContextAccessor>();
        }


        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            tracingContext.Release(segmentContextAccessor.Context);
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var requestUri = channel.RemoteAddress.Uri;
            var operationName = requestUri.ToString();
            var networkAddress = $"{requestUri.Host}:{requestUri.Port}";

            //SoapICarrierHeader soapICarrierHeader = new SoapICarrierHeader();
            var context = tracingContext.CreateExitSegmentContext(operationName, networkAddress, new SoapICarrierHeaderCollection(request.Headers));
            context.Span.SpanLayer = SpanLayer.RPC_FRAMEWORK;
            context.Span.Component = Components.SOAPCLIENT;
            context.Span.AddTag(Tags.RPC_METHOD, operationName);
            context.Span.AddTag(Tags.RPC_TYPE, "soap");

            //string header = SoapHeaderUtils.WrapSoapICarrierHeader(soapICarrierHeader);
            //request.Headers.Insert(0, new SkyApmMessageHeader(header));

            request.Headers.Insert(0, MessageHeader.CreateHeader("SkyApmHeaderString", "", ""));

            return Guid.NewGuid();
        }

        private class SoapICarrierHeaderCollection : ICarrierHeaderCollection
        {
            private readonly MessageHeaders soapICarrierHeader;

            public SoapICarrierHeaderCollection(MessageHeaders headers)
            {
                soapICarrierHeader = headers;
            }

            public void Add(string key, string value)
            {
                soapICarrierHeader.Add(MessageHeader.CreateHeader(key, string.Empty, value));
            }

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
#endif