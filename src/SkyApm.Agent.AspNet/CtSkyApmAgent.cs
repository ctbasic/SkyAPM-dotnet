using System;
using System.Threading;
using System.Threading.Tasks;
#if NETSTANDARD
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
#endif
using SkyApm.Abstractions.Common;

namespace SkyApm.Agent.AspNet
{
    internal class CtSkyApmAgent: ICtSkyApmAgent
    {
        private readonly IInstrumentStartup instrumentStartup;

        public CtSkyApmAgent(IInstrumentStartup instrumentStartup)
        {
            this.instrumentStartup = instrumentStartup;
        }

        public Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
#if NETSTANDARD
            AsyncContext.Run(() => instrumentStartup.StartAsync());
            
#else
            Task.Run(() => { instrumentStartup.StartAsync(cancellationToken); });
#endif
            return CustomTaskUtils.ReturnCompletedTask();
        }

        public Task StopAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
#if NETSTANDARD
            AsyncContext.Run(() => instrumentStartup.StopAsync());

#else
            Task.Run(() =>
            {
                instrumentStartup.StopAsync(cancellationToken);
            });
#endif
            return Task.Delay(TimeSpan.FromSeconds(2));
        }
    }
}
