using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SkyApm.Sample2.NetCoreWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }


        public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
    .UseSetting(WebHostDefaults.HostingStartupAssembliesKey, "CtSkyAPM.Agent.AspNetCore")
             .ConfigureServices((hostBuilderContext, services) =>
             {
                 #region  此部分内容必须置顶

                 IConfiguration config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .Build();

                 Thrift.Client.ClientInstance.Instance(config);

                 hostBuilderContext.Configuration = config;

                 #endregion
             })
        .UseStartup<Startup>()
        .UseUrls("http://*:5004")
        .Build();
    }
}
