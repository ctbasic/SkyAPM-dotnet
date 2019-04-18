﻿/*
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
using CommonServiceLocator;
using System.Web;
#if !NET_FX45
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
#endif

#if NET_FX45
using System.Threading.Tasks;
#endif
using SkyApm.Agent.AspNet.Extensions;
using SkyApm.Utilities.DependencyInjectionEx.Dependency;

namespace SkyApm.Agent.AspNet
{
    public class InstrumentModule : IHttpModule
    {
        public InstrumentModule()
        {
//#if NET_FX45
//            var serviceProvider = new AutofacServiceCollection().AddSkyAPMCore().BuildServiceProvider();
//            var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);
//#else
//            var serviceProvider = new ServiceCollection().AddSkyAPMCore().BuildServiceProvider();
//            var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);
//#endif
//            ServiceLocator.SetLocatorProvider(() => serviceLocatorProvider);
        }

        public void Init(HttpApplication application)
        {
//            var startup = ServiceLocator.Current.GetInstance<IInstrumentStartup>();
//            if (startup == null)
//            {
//                throw new NotImplementedException("IInstrumentStartup instance is null");
//            }

//#if NET_FX45
//            Task.Run(() =>
//            {
//                startup.StartAsync().Wait();
//            });
//#else
//                AsyncContext.Run(() => startup.StartAsync());
//#endif

            var requestCallback = ServiceLocator.Current.GetInstance<InstrumentRequestCallback>();
            application.BeginRequest += requestCallback.ApplicationOnBeginRequest;
            application.EndRequest += requestCallback.ApplicationOnEndRequest;
        }

        public void Dispose()
        {
//            var startup = ServiceLocator.Current.GetInstance<IInstrumentStartup>();
//#if NET_FX45
//            Task.Run(() =>
//            {
//                startup.StopAsync().Wait();
//            });
//#else
//                AsyncContext.Run(() => startup.StopAsync());
//#endif
        }
    }
}