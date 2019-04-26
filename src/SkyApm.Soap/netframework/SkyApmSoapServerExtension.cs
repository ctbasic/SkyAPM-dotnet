//#if !NETSTANDARD
//using System;
//using System.IO;
//using System.Web.Services.Protocols;
//using System.Xml;
//using CommonServiceLocator;
//using SkyApm.Common;
//using SkyApm.Tracing;
//using SkyApm.Tracing.Segments;

//namespace SkyApm.Soap
//{

//    public class SkyApmSoapServerExtension : SoapExtension
//    {
//        Stream requestStream;
//        Stream responseStream;
//        XmlDocument requestXml;
//        XmlDocument responseXml;

//        private static SkyApm.Logging.ILogger logger = ServiceLocator.Current.GetInstance<SkyApm.Logging.ILoggerFactory>().CreateLogger(typeof(SkyApmSoapClientExtension));
//        private readonly ITracingContext tracingContext;
//        private readonly IEntrySegmentContextAccessor segmentContextAccessor;

//        public SkyApmSoapServerExtension()
//        {
//            tracingContext = ServiceLocator.Current.GetInstance<ITracingContext>();
//            segmentContextAccessor = ServiceLocator.Current.GetInstance<IEntrySegmentContextAccessor>();
//        }

//        public override void ProcessMessage(SoapMessage message)
//        {
//            var isServerMessage = message is SoapServerMessage;

//            switch (message.Stage)
//            {
//                case SoapMessageStage.BeforeDeserialize:
//                    CopyStream(requestStream, responseStream);
//                    requestXml = SetContentToXml(responseStream);
//                    break;
//                case SoapMessageStage.AfterDeserialize: // 当作为客户端访问外部Soap接口时最后一步

//                    if (isServerMessage)
//                    {
//                        SoapServerReceive(message);
//                    }

//                    break;
//                case SoapMessageStage.BeforeSerialize:
//                    break;
//                case SoapMessageStage.AfterSerialize: // 当作为服务端被外部访问时最后一步
//                    responseXml = SetContentToXml(responseStream);
//                    CopyStream(responseStream, requestStream);

//                    if (isServerMessage)
//                    {
//                        SoapServerSend();
//                    }
//                    break;
//            }
//        }

//        /// <summary>
//        ///     Soap服务端响应发送处理
//        /// </summary>
//        private void SoapServerSend()
//        {
//            try
//            {
//                tracingContext.Release(segmentContextAccessor.Context);
//            }
//            catch (Exception e)
//            {
//                logger.Error("SoapServerSend error", e);
//            }
//        }

//        /// <summary>
//        ///     Soap服务端接收处理
//        /// </summary>
//        /// <param name="message"></param>
//        private void SoapServerReceive(SoapMessage message)
//        {
//            try
//            {
//                var operationName = "";
//                var soapICarrierHeader = message.Headers.ExtractSoapICarrierHeader();
//                var segmentContext = tracingContext.CreateEntrySegmentContext(operationName,
//                    new SoapICarrierHeaderCollection(soapICarrierHeader));
//                segmentContext.Span.SpanLayer = SpanLayer.RPC_FRAMEWORK;
//                segmentContext.Span.Component = Components.ASPNETCORE;
//                segmentContext.Span.Peer = new StringOrIntValue("192.168.1.1:11");
//                segmentContext.Span.AddTag(Tags.RPC_METHOD, operationName);
//                segmentContext.Span.AddTag(Tags.RPC_TYPE, "soap");
//                segmentContext.Span.AddLog(
//                    LogEvent.Event("Thrift Hosting BeginRequest"),
//                    LogEvent.Message(
//                        $"Request starting thrift {operationName}"));
//            }
//            catch (Exception e)
//            {
//                logger.Error("SoapServerReceive error", e);
//            }
//        }

//        public override Stream ChainStream(Stream stream)
//        {
//            requestStream = stream;
//            responseStream = new MemoryStream();
//            return responseStream;
//        }

//        private void CopyStream(Stream requestStream, Stream responseStream)
//        {
//            TextReader reader = new StreamReader(requestStream);
//            TextWriter writer = new StreamWriter(responseStream);
//            writer.WriteLine(reader.ReadToEnd());
//            writer.Flush();
//        }

//        private XmlDocument SetContentToXml(Stream stream)
//        {
//            var xml = new XmlDocument();
//            stream.Position = 0;
//            var reader = new StreamReader(stream);
//            xml.LoadXml(reader.ReadToEnd());
//            stream.Position = 0;
//            return xml;
//        }

//        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
//        {
//            return null;
//        }

//        public override object GetInitializer(Type serviceType)
//        {
//            return serviceType;
//        }

//        public override void Initialize(object initializer)
//        {
//        }
//    }
//}
//#endif