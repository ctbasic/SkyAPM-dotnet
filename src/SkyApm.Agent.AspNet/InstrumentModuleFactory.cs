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

using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System.Web;
using CommonServiceLocator;
using SkyApm.Agent.AspNet;
using SkyApm.Agent.AspNet.Extensions;
#if !NET_FX45
using Microsoft.Extensions.DependencyInjection;
#endif

#if NET_FX45
using SkyApm.Utilities.DependencyInjectionEx.Dependency;
#endif

[assembly: PreApplicationStartMethod(typeof(InstrumentModuleFactory), nameof(InstrumentModuleFactory.Create))]

namespace SkyApm.Agent.AspNet
{
    public class InstrumentModuleFactory
    {
        public static void Create()
        {
#if NET_FX45
            var serviceProvider = new AutofacServiceCollection().AddSkyAPMCore().BuildServiceProvider();
            var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);
#else
            var serviceProvider = new ServiceCollection().AddSkyAPMCore().BuildServiceProvider();
            var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);
#endif
            ServiceLocator.SetLocatorProvider(() => serviceLocatorProvider);

            var ctSkyApmAgent = ServiceLocator.Current.GetInstance<ICtSkyApmAgent>();
            ctSkyApmAgent.StartAsync();

            DynamicModuleUtility.RegisterModule(typeof(InstrumentModule));
        }
    }
}