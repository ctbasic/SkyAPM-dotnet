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
using Serilog.Events;
using System;

namespace SkyApm.Utilities.Logging
{
#if NETSTANDARD
    using Microsoft.Extensions.Logging;
    using Serilog.Events;
    using MSLogger = Microsoft.Extensions.Logging.ILogger;

    internal class DefaultLogger : SkyApm.Logging.ILogger
    {
        private readonly MSLogger _readLogger;
        private readonly LogEventLevel _level;

        public DefaultLogger(MSLogger readLogger, LogEventLevel level)
        {
            _readLogger = readLogger;
            _level = level;
        }

        public void Debug(string message)
        {
            if (_level <= LogEventLevel.Debug)
                _readLogger.LogDebug(message);
        }

        public void Information(string message)
        {
            if (_level <= LogEventLevel.Information)
                _readLogger.LogInformation(message);
        }

        public void Warning(string message)
        {
            if (_level <= LogEventLevel.Warning)
                _readLogger.LogWarning(message);
        }

        public void Error(string message, Exception exception)
        {
            if (_level <= LogEventLevel.Error)
                _readLogger.LogError(message + Environment.NewLine + exception);
        }

        public void Trace(string message)
        {
            if (_level <= LogEventLevel.Verbose)
                _readLogger.LogTrace(message);
        }
    }
#else
    using Log4NetLogger = log4net.ILog;

    internal class DefaultLogger : SkyApm.Logging.ILogger
    {
        private readonly Log4NetLogger _readLogger;
        private readonly LogEventLevel _level;

        public DefaultLogger(log4net.ILog readLogger, LogEventLevel level)
        {
            _readLogger = readLogger;
            _level = level;
        }

        public void Debug(string message)
        {
            if (_level <= LogEventLevel.Debug)
                _readLogger.Debug(message);
        }

        public void Information(string message)
        {
            if (_level <= LogEventLevel.Information)
                _readLogger.Info(message);
        }

        public void Warning(string message)
        {
            if (_level <= LogEventLevel.Warning)
                _readLogger.Warn(message);
        }

        public void Error(string message, Exception exception)
        {
            if (_level <= LogEventLevel.Error)
                _readLogger.Error(message + Environment.NewLine + exception);
        }

        public void Trace(string message)
        {
            if (_level <= LogEventLevel.Verbose)
                _readLogger.Debug(message);
        }
    }
#endif

}