using System;
using CommonServiceLocator;
using SkyApm.Utilities.DependencyInjection;

namespace SkyApm.Thrift
{
#if NETSTANDARD
        using Microsoft.Extensions.DependencyInjection;
#endif

        public static class SkyWalkingBuilderExtensions
        {
            public static SkyApmExtensions AddThriftTrace(this SkyApmExtensions extensions)
            {
                if (extensions == null)
                {
                    throw new ArgumentNullException(nameof(extensions));
                }
#if NETSTANDARD

                var serviceProvider = extensions.Services.BuildServiceProvider();
                var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);

#else
                var serviceProvider = extensions.Services.BuildServiceProvider();
                var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);
#endif
                if (!ServiceLocator.IsLocationProviderSet)
                {
                    ServiceLocator.SetLocatorProvider(() => serviceLocatorProvider);
                }                

                return extensions;
            }
        }
}
