using System;
using System.Threading;
using System.Threading.Tasks;
#if !NET_FX45
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
#endif
using SkyApm.Abstractions.Common;

namespace SkyApm.Agent.AspNet
{
    public class CtSkyApmAgent: ICtSkyApmAgent
    {
        private readonly IInstrumentStartup instrumentStartup;

        public CtSkyApmAgent(IInstrumentStartup instrumentStartup)
        {
            this.instrumentStartup = instrumentStartup;
        }

        public Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
#if NET_FX45
            Task.Run(() => { instrumentStartup.StartAsync(cancellationToken); });
#else
                AsyncContext.Run(() => instrumentStartup.StartAsync());
#endif
            return CustomTaskUtils.ReturnCompletedTask();
        }

        public Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
#if NET_FX45
            Task.Run(() =>
            {
                instrumentStartup.StopAsync(cancellationToken);
            });
#else
                AsyncContext.Run(() => instrumentStartup.StopAsync());
#endif
            return Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
