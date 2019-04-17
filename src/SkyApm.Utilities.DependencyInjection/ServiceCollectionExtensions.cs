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



namespace SkyApm.Utilities.DependencyInjection
{
#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
    public static class ServiceCollectionExtensions
    {
        public static SkyApmExtensions AddSkyApmExtensions(this IServiceCollection services)
        {
            return new SkyApmExtensions(services);
        }
    }
#else
    using Autofac;
    public static class ServiceCollectionExtensions
    {
        public static SkyApmExtensions AddSkyApmExtensions(this IServiceCollectionContainer services)
        {
            return new SkyApmExtensions(services);
        }
    }
#endif
}