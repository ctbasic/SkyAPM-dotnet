#if NET45
using log4net;
#endif
#if NET_CORE
using Microsoft.Extensions.Logging;
#endif
using zipkin4net;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport.Http;

namespace thrift
{
    public class ZipkinManager
    {
        private const string LOGGER_NAME = "Zipkin";

        public static string serviceName = SerializerUtils.DefaultServiceName;

#if NET45
        public static void Start(ILog logger,string serviceCode,string connectionStr)
        {
            if (TraceManager.Started)
            {
                return;
            }
            serviceName=serviceCode;
            zipkin4net.ILogger zipkinLogger = new TracingLogger(logger);
            TraceManager.SamplingRate = 1.0f;
            var httpSender = new HttpZipkinSender("http://192.168.101.47:9411", "application/json");
            var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new DefaultStatistics());
            TraceManager.RegisterTracer(tracer);
            TraceManager.Start(zipkinLogger);
        }
#endif

#if NET_CORE
        public static void Start(string serviceCode, ILoggerFactory loggerFactory, string connectionStr)
        {
            if (TraceManager.Started)
            {
                return;
            }
            serviceName = serviceCode;
            Microsoft.Extensions.Logging.ILogger logger = loggerFactory.CreateLogger(LOGGER_NAME);
            zipkin4net.ILogger zipkinLogger = new TracingLogger(logger);
            TraceManager.SamplingRate = 1.0f;
            var httpSender = new HttpZipkinSender("http://192.168.101.47:9411", "application/json");
            var tracer = new ZipkinTracer(httpSender, new JSONSpanSerializer(), new DefaultStatistics());
            TraceManager.RegisterTracer(tracer);
            TraceManager.Start(zipkinLogger);
        }
#endif
        public static void Stop()
        {
            TraceManager.Stop();
        }
    }
}

