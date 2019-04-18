#if NET45
using log4net;

namespace thrift
{
    public class TracingLogger : zipkin4net.ILogger
    {
        private readonly ILog logger;

        public TracingLogger(ILog logger)
        {
            this.logger = logger;
        }
        public void LogError(string message)
        {
            logger.Error(message);
        }
        public void LogInformation(string message)
        {
            logger.Info(message);
        }
        public void LogWarning(string message)
        {
            logger.Warn(message);
        }
    }
}
#endif

#if NET_CORE
using Microsoft.Extensions.Logging;

namespace thrift
{
    public class TracingLogger : zipkin4net.ILogger
    {
        private readonly ILogger _logger;

        public TracingLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void LogError(string message)
        {
            _logger.LogError(message);
        }
        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }
        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }
    }
}
#endif