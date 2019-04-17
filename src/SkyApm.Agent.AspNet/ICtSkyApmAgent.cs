using System.Threading;
using System.Threading.Tasks;

namespace SkyApm.Agent.AspNet
{
    public interface ICtSkyApmAgent
    {
        Task StartAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task StopAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
