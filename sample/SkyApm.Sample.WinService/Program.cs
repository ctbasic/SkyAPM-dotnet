using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using SkyApm.Agent.AspNet;
using SkyApm.Sample.WinService.soapapmservice;

namespace SkyApm.Sample.WinService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            CtSkyApmNetFxAgent.StartAsync();

            while (!CtSkyApmNetFxAgent.Initialized)
            {
                Task.Delay(1000);
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new HostService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
