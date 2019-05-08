using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkyApm.Agent.NetCoreHost.Extensions;
using System;


namespace SkyApm.Sample2.NetCoreThrift
{
    class Program
    {
        static void Main(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder();
            hostBuilder
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    #region  此部分内容必须置顶

                    IConfiguration config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("Config/AppSettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables().Build();

                    hostBuilderContext.Configuration = config;

                    Thrift.Server.Server.Instance(config);

                    #endregion

                    services.AddSingleton<IHostedService, ThriftHostService>();
                })
                .AddSkyAPM()
                .Build().Run();



        }
    }
}
