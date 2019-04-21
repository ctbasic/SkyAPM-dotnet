
#if !NETSTANDARD
using System;
using Autofac;

namespace SkyApm.Utilities.DependencyInjectionEx.Dependency
{
    public class AutofacServiceProvider : IServiceProvider
    {
        private readonly IContainer autofaContainer;

        public AutofacServiceProvider(IContainer container)
        {
            autofaContainer = container;
        }

        public object GetService(Type serviceType)
        {
            return autofaContainer.Resolve(serviceType);
        }
    }
}

#endif
