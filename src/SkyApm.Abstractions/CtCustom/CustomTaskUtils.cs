using System.Threading.Tasks;

namespace SkyApm.Abstractions.Common
{
    public static class CustomTaskUtils
    {
        public static Task ReturnCompletedTask()
        {
#if NET_FX
            return Task.FromResult(0);
#else
            return Task.CompletedTask;
#endif
        }
    }
}
