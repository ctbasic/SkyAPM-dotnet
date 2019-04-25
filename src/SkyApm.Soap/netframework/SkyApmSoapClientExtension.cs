#if !NETSTANDARD

#region

using System;
using System.IO;
using System.Web.Services.Protocols;
using System.Xml;
using CommonServiceLocator;
using SkyApm.Common;
using SkyApm.Logging;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;

#endregion

namespace SkyApm.Soap
{
    public class SkyApmSoapClientExtension : SoapExtension
    {
        Stream requestStream;
        Stream responseStream;
        XmlDocument requestXml;
        XmlDocument responseXml;

        private static readonly ILogger logger = ServiceLocator.Current.GetInstance<ILoggerFactory>().CreateLogger(typeof(SkyApmSoapClientExtension));
        private readonly ITracingContext tracingContext;
        private readonly IExitSegmentContextAccessor segmentContextAccessor;

        public SkyApmSoapClientExtension()
        {
            tracingContext = ServiceLocator.Current.GetInstance<ITracingContext>();
            segmentContextAccessor = ServiceLocator.Current.GetInstance<IExitSegmentContextAccessor>();
        }

        public override void ProcessMessage(SoapMessage message)
        {
            var isClientMessage = message is SoapClientMessage;

            switch (message.Stage)
            {
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(requestStream, responseStream);
                    requestXml = SetContentToXml(responseStream);
                    break;
                case SoapMessageStage.AfterDeserialize: // 当作为客户端访问外部Soap接口时最后一步

                    if (isClientMessage)
                    {
                        SoapClientReceive();
                    }

                    break;
                case SoapMessageStage.BeforeSerialize:
                    if (isClientMessage)
                    {
                        SoapClientSend(message);
                    }

                    break;
                case SoapMessageStage.AfterSerialize: // 当作为服务端被外部访问时最后一步
                    responseXml = SetContentToXml(responseStream);
                    CopyStream(responseStream, requestStream);

                    break;
            }
        }

        private void SoapClientReceive()
        {
            tracingContext.Release(segmentContextAccessor.Context);
        }

        private void SoapClientSend(SoapMessage message)
        {
            try
            {
                var requestUri = new Uri(message.Url);
                var operationName = message.MethodInfo.Name;
                var networkAddress = $"{requestUri.Host}:{requestUri.Port}";

                SoapICarrierHeader soapICarrierHeader = new SoapICarrierHeader();
                var context = tracingContext.CreateExitSegmentContext(operationName, networkAddress,
                    new SoapICarrierHeaderCollection(soapICarrierHeader));

                //context.Span.SpanLayer = SpanLayer.RPC_FRAMEWORK;
                //context.Span.Component = Components.SOAPCLIENT;
                //context.Span.AddTag(Tags.RPC_METHOD, operationName);
                //context.Span.AddTag(Tags.RPC_TYPE, "soap");

                context.Span.SpanLayer = SpanLayer.HTTP;
                context.Span.Component = Common.Components.HTTPCLIENT;
                context.Span.AddTag(Common.Tags.URL, requestUri.ToString());
                context.Span.AddTag(Common.Tags.PATH, requestUri.ToString());
                context.Span.AddTag(Common.Tags.HTTP_METHOD, "POST");


                string header = SoapHeaderUtils.WrapSoapICarrierHeader(soapICarrierHeader);
                message.Headers.Insert(0, new SkyApmSoapHeader {SkyApmHeaderString = header});
            }
            catch (Exception e)
            {
                logger.Error("SoapClientSend error", e);
            }
        }

        public override Stream ChainStream(Stream stream)
        {
            requestStream = stream;
            responseStream = new MemoryStream();
            return responseStream;
        }

        private void CopyStream(Stream requestStream, Stream responseStream)
        {
            TextReader reader = new StreamReader(requestStream);
            TextWriter writer = new StreamWriter(responseStream);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }

        private XmlDocument SetContentToXml(Stream stream)
        {
            var xml = new XmlDocument();
            stream.Position = 0;
            var reader = new StreamReader(stream);
            xml.LoadXml(reader.ReadToEnd());
            stream.Position = 0;
            return xml;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override object GetInitializer(Type serviceType)
        {
            return serviceType;
        }

        public override void Initialize(object initializer)
        {
        }
    }
}
#endif