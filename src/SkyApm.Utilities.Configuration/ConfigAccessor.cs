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
using System.Reflection;
using SkyApm.Config;

namespace SkyApm.Utilities.Configuration
{
#if NET_FX45
    using System.IO;
    using Newtonsoft.Json.Linq;

    public class ConfigAccessor : IConfigAccessor
    {
        readonly JObject _configuration;


        public ConfigAccessor(IEnvironmentProvider environmentProvider)
        {
            _configuration = new JObject();
            _configuration.AddSkyWalkingDefaultConfig();

            string configFile = AppDomain.CurrentDomain.BaseDirectory + "skyapm.json";
            if (File.Exists(configFile))
            {
                string text = System.IO.File.ReadAllText(configFile);
                _configuration.Merge(JObject.Parse(text));
                return;
            }

            configFile = AppDomain.CurrentDomain.BaseDirectory + "Config/Complex/skyapm.json";
            if (File.Exists(configFile))
            {
                string text = System.IO.File.ReadAllText(configFile);
                _configuration.Merge(JObject.Parse(text));
            }
        }

        public T Get<T>() where T : class, new()
        {
            var config = typeof(T).GetCustomAttribute<ConfigAttribute>();
            var sections = config.Sections;
            var childJObject = _configuration;
            foreach (var section in sections)
            {
                childJObject = (JObject)childJObject[section];
            }

            return childJObject.ToObject<T>();
        }

        public T Value<T>(string key, params string[] sections)
        {
            var childJObject = _configuration;
            foreach (var section in sections)
            {
                childJObject = (JObject)_configuration[section];
            }

            return childJObject.ToObject<T>();
        }
    }
#else
    using Microsoft.Extensions.Configuration;
    using System.Linq.Expressions;
    public class ConfigAccessor : IConfigAccessor
    {
        private readonly IConfiguration _configuration;

        public ConfigAccessor(IConfigurationFactory factory)
        {
            _configuration = factory.Create();
        }

        public T Get<T>() where T : class, new()
        {
            var config = typeof(T).GetCustomAttribute<ConfigAttribute>();
            var instance = New<T>.Instance();
            _configuration.GetSection(config.GetSections()).Bind(instance);
            return instance;
        }

        public T Value<T>(string key, params string[] sections)
        {
            var config = new ConfigAttribute(sections);
            return _configuration.GetSection(config.GetSections()).GetValue<T>(key);
        }

        /// <summary>
        /// high performance
        /// </summary>
        private static class New<T> where T : new()
        {
            public static readonly Func<T> Instance = Expression.Lambda<Func<T>>
            (
                Expression.New(typeof(T))
            ).Compile();
        }
    }
#endif
}