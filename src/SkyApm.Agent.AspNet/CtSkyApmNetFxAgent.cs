using System.Threading;
using System.Threading.Tasks;
using CommonServiceLocator;
using SkyApm.Agent.AspNet.Extensions;
using SkyApm.CtCustom;
using SkyApm.Tracing;
#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
#else
using SkyApm.Utilities.DependencyInjection.Dependency;
#endif

namespace SkyApm.Agent.AspNet
{
    /// <summary>
    /// 客户端启动静态类
    /// </summary>
    public class CtSkyApmNetFxAgent
    {
        static CtSkyApmNetFxAgent()
        {
#if NETSTANDARD
            var serviceProvider = new ServiceCollection().AddSkyAPMCore().BuildServiceProvider();
            var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);


#else
            var serviceProvider = new AutofacServiceCollection().AddSkyAPMCore().BuildServiceProvider();
            var serviceLocatorProvider = new ServiceProviderLocator(serviceProvider);
#endif
            ServiceLocator.SetLocatorProvider(() => serviceLocatorProvider);
        }

        public static Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var ctSkyApmAgent = ServiceLocator.Current.GetInstance<ICtSkyApmAgent>();
            return ctSkyApmAgent.StartAsync(cancellationToken);
        }

        public static Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var ctSkyApmAgent = ServiceLocator.Current.GetInstance<ICtSkyApmAgent>();
            return ctSkyApmAgent.StopAsync(cancellationToken);
        }


        public static bool Initialized => ServiceLocator.Current.GetInstance<IRuntimeEnvironment>().Initialized;

        /// <summary>
        /// 当前tranceId
        /// </summary>
        public static string CurrentTranceId =>
            ServiceLocator.Current.GetInstance<ICustomSegmentContextAccessor>().CurrentTraceId;
    }
}
