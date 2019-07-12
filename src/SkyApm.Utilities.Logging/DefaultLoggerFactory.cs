/*
 * Licensed to the SkyAPM under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The SkyAPM licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using SkyApm.Config;


namespace SkyApm.Utilities.Logging
{
#if NETSTANDARD
    using Serilog;
    using Serilog.Events;
    using Microsoft.Extensions.Logging;

    using MSLoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;

    public class DefaultLoggerFactory : SkyApm.Logging.ILoggerFactory
    {
        private const string outputTemplate =
            @"{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{ServiceName}] [{Level}] {SourceContext} : {Message}{NewLine}{Exception}";

        private readonly MSLoggerFactory _loggerFactory;
        private readonly LoggingConfig _loggingConfig;
        private LogEventLevel _level;

        public DefaultLoggerFactory(IConfigAccessor configAccessor, MSLoggerFactory mSLoggerFactory)
        {
         //   _loggerFactory = new MSLoggerFactory();
            _loggerFactory = mSLoggerFactory;
            _loggingConfig = configAccessor.Get<LoggingConfig>();

             // var instrumentationConfig = configAccessor.Get<InstrumentConfig>();

           _level = EventLevel(_loggingConfig.Level);

            //_loggerFactory.AddSerilog(new LoggerConfiguration().MinimumLevel.Verbose().Enrich                .WithProperty("SourceContext", null).Enrich
            //    .WithProperty(nameof(instrumentationConfig.ServiceName),
            //        instrumentationConfig.ServiceName ?? instrumentationConfig.ApplicationCode).Enrich
            //    .FromLogContext().WriteTo.RollingFile(_loggingConfig.FilePath, level, outputTemplate, null, 1073741824,
            //        31, null, false, false, TimeSpan.FromMilliseconds(500)).CreateLogger());
        }

        public SkyApm.Logging.ILogger CreateLogger(Type type)
        {
            return new DefaultLogger(_loggerFactory.CreateLogger(type),_level);
        }

        private static LogEventLevel EventLevel(string level)
        {
            return LogEventLevel.TryParse<LogEventLevel>(level, out var logEventLevel)
                ? logEventLevel
                : LogEventLevel.Error;
        }
    }
#else
    using log4net;
    using Serilog.Events;

    public class DefaultLoggerFactory : SkyApm.Logging.ILoggerFactory
    {
        private LogEventLevel _level;

        public DefaultLoggerFactory(IConfigAccessor configAccessor)
        {
            var _loggingConfig = configAccessor.Get<LoggingConfig>();
            _level = EventLevel(_loggingConfig.Level);
        }

        public SkyApm.Logging.ILogger CreateLogger(Type type)
        {
            var logger = LogManager.GetLogger(type);
            return new DefaultLogger(logger, _level);
        }

        private static LogEventLevel EventLevel(string level)
        {
            return LogEventLevel.TryParse<LogEventLevel>(level, out var logEventLevel)
                ? logEventLevel
                : LogEventLevel.Error;
        }
    }
#endif
}