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

using System.Collections.Generic;
using System.IO;
using SkyApm.Config;

namespace SkyApm.Utilities.Configuration
{
#if NET_FX45
    using Newtonsoft.Json.Linq;
    internal static class ConfigurationBuilderExtensions
    {
        public static JObject AddSkyWalkingDefaultConfig(this JObject builder)
        {
            var defaultLogFile = Path.Combine("logs", "skyapm-{Date}.log");
            if (builder == null)
            {
                builder = new JObject();
            }
            builder["SkyWalking"] = new JObject();
            builder["SkyWalking"]["Namespace"] = string.Empty;
            builder["SkyWalking"]["ServiceName"] = "My_Service";
            builder["SkyWalking"]["HeaderVersions"] = new JArray();
            builder["SkyWalking"]["HeaderVersions"].AddAnnotation(HeaderVersions.SW6);
            builder["SkyWalking"]["Sampling"] = new JObject();
            builder["SkyWalking"]["Sampling"]["SamplePer3Secs"] = "-1";
            builder["SkyWalking"]["Sampling"]["Percentage"] = "-1";
            builder["SkyWalking"]["Logging"] = new JObject();
            builder["SkyWalking"]["Logging"]["Level"] = "Information";
            builder["SkyWalking"]["Logging"]["FilePath"] = defaultLogFile;
            builder["SkyWalking"]["Transport"] = new JObject();
            builder["SkyWalking"]["Transport"]["Interval"] = "3000";
            builder["SkyWalking"]["Transport"]["ProtocolVersion"] = ProtocolVersions.V6;
            builder["SkyWalking"]["Transport"]["QueueSize"] = "30000";
            builder["SkyWalking"]["Transport"]["BatchSize"] = "3000";
            builder["SkyWalking"]["Transport"]["gRPC"] = new JObject();
            builder["SkyWalking"]["Transport"]["gRPC"]["Servers"] = "192.168.101.70:11800";
            builder["SkyWalking"]["Transport"]["gRPC"]["Timeout"] = "10000";
            builder["SkyWalking"]["Transport"]["gRPC"]["ReportTimeout"] = "600000";
            builder["SkyWalking"]["Transport"]["gRPC"]["ConnectTimeout"] = "10000";
            return builder;
        }
    }
#else
using Microsoft.Extensions.Configuration;
    internal static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddSkyWalkingDefaultConfig(this IConfigurationBuilder builder)
        {
            var defaultLogFile = Path.Combine("logs", "skyapm-{Date}.log");
            var defaultConfig = new Dictionary<string, string>
            {
                {"SkyWalking:Namespace", string.Empty},
                {"SkyWalking:ServiceName", "My_Service"},
                {"SkyWalking:HeaderVersions:0", HeaderVersions.SW6},
                {"SkyWalking:Sampling:SamplePer3Secs", "-1"},
                {"SkyWalking:Sampling:Percentage", "-1"},
                {"SkyWalking:Logging:Level", "Information"},
                {"SkyWalking:Logging:FilePath", defaultLogFile},
                {"SkyWalking:Transport:Interval", "3000"},
                {"SkyWalking:Transport:ProtocolVersion", ProtocolVersions.V6},
                {"SkyWalking:Transport:QueueSize", "30000"},
                {"SkyWalking:Transport:BatchSize", "3000"},
                {"SkyWalking:Transport:gRPC:Servers", "192.168.101.70:11800"},
                {"SkyWalking:Transport:gRPC:Timeout", "10000"},
                {"SkyWalking:Transport:gRPC:ReportTimeout", "600000"},
                {"SkyWalking:Transport:gRPC:ConnectTimeout", "10000"}
            };
            return builder.AddInMemoryCollection(defaultConfig);
        }
    }
#endif
}