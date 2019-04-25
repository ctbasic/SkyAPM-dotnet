using System;
using System.Collections.Generic;
using CommonServiceLocator;

namespace SkyApm.Agent.NetCoreHost
{
    using Microsoft.Extensions.DependencyInjection;
    internal class ServiceProviderLocator : IServiceLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type serviceType) => _serviceProvider.GetService(serviceType);

        public object GetInstance(Type serviceType) => _serviceProvider.GetService(serviceType);

        public object GetInstance(Type serviceType, string key) => GetInstance(serviceType);

        public IEnumerable<object> GetAllInstances(Type serviceType) => _serviceProvider.GetServices(serviceType);

        public TService GetInstance<TService>() => (TService)GetInstance(typeof(TService));

        public TService GetInstance<TService>(string key) => (TService)GetInstance(typeof(TService));

        public IEnumerable<TService> GetAllInstances<TService>() => _serviceProvider.GetServices<TService>();
    }
}
