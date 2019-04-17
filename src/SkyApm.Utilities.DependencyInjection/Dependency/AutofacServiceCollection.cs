#if NET_FX

using System;
using Autofac;
using SkyApm.Utilities.DependencyInjection;

namespace SkyApm.Utilities.DependencyInjectionEx.Dependency
{
    public class AutofacServiceCollection:IServiceCollectionContainer
    {

        private readonly ContainerBuilder autofacContainerBuilder;

        public AutofacServiceCollection()
        {
            autofacContainerBuilder = new ContainerBuilder();
        }

        private IContainer container;

        public IServiceCollectionContainer AddSingleton<TService, TImplementation>()
        {
            autofacContainerBuilder.RegisterType<TImplementation>().As<TService>().SingleInstance();
            return this;
        }

        public IServiceCollectionContainer AddSingleton<TImplementation>()
        {
            autofacContainerBuilder.RegisterType<TImplementation>().SingleInstance();
            return this;
        }

        public IServiceCollectionContainer AddSingleton<TService>(TService implementationType) where TService : class
        {
            autofacContainerBuilder.RegisterInstance(implementationType).SingleInstance();
            return this;
        }

        public IServiceCollectionContainer AddSingletonAfterResolve<TService>(Type resolveType)
        {
            autofacContainerBuilder.Register<TService>(c => (TService)c.Resolve(resolveType)).SingleInstance();
            return this;
        }


        public IServiceProvider BuildServiceProvider()
        {
            if (container == null)
            {
                container = autofacContainerBuilder.Build();
            }
            return new AutofacServiceProvider(container);
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}
#endif