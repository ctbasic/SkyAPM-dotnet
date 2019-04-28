#if NETSTANDARD
using CommonServiceLocator;
using SkyApm.Tracing;
using System;
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
            var actionUri = new Uri(request.Headers.Action);
            string actionName = actionUri.PathAndQuery.Replace("/", "");
            var operationName = requestUri.ToString() + "/" + actionName;
            var networkAddress = $"{requestUri.Host}:{requestUri.Port}";

            SoapICarrierHeaders soapICarrierHeader = new SoapICarrierHeaders();
            var context = tracingContext.CreateExitSegmentContext(operationName, networkAddress, new SoapICarrierHeaderCollection(soapICarrierHeader));
            context.Span.SpanLayer = SpanLayer.HTTP;
            context.Span.Component = Components.HTTPCLIENT;

            context.Span.AddTag(Common.Tags.URL, requestUri.ToString()+"/"+ actionName);
            context.Span.AddTag(Common.Tags.PATH, actionUri.PathAndQuery + "/" + actionName);
            context.Span.AddTag(Common.Tags.HTTP_METHOD, "POST");

            string header = soapICarrierHeader.EncodeSoapICarrierHeader();
            request.Headers.Insert(0, new SkyApmMessageHeader(header));

            return Guid.NewGuid();
        }
    }
}
#endif