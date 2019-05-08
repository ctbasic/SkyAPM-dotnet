using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SkyApm.Agent.NetCoreHost.Extensions;


namespace Thrift.ServiceWin_NETCore
{

    public class mylogger : SkyApm.Logging.ILoggerFactory
    {
        public mylogger( )
        {
        }

        public SkyApm.Logging.ILogger CreateLogger(Type type)
        {
            return new myDefaultLogger();
        }

    }

    internal class myDefaultLogger : SkyApm.Logging.ILogger
    {

        public myDefaultLogger( )
        {
        }

        public void Debug(string message)
        {
            Console.WriteLine(message); 
        }

        public void Information(string message)
        {
            Console.WriteLine(message);
        }

        public void Warning(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string message, Exception exception)
        {
            Console.WriteLine(message + Environment.NewLine + exception);
        }

        public void Trace(string message)
        {
            Console.WriteLine(message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("服务d  3启动");

            //IConfigurationRoot config = new ConfigurationBuilder()
            //      .AddJsonFile("Config/AppSettings.json", optional: false, reloadOnChange: true)
            //      .Build();
            //Server.Server.Instance(config);

            IHostBuilder hostBuilder = new HostBuilder();
            hostBuilder
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    #region  此部分内容必须置顶

                    IConfiguration config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("Config/AppSettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables().Build();

                    hostBuilderContext.Configuration = config;

                    Server.Server.Instance(config);

                    #endregion

                    services.AddSingleton<IHostedService, ThriftHostService>();
            //        services.AddSingleton<SkyApm.Logging.ILoggerFactory, mylogger>();
                })
                .AddSkyAPM()
                .Build().Run();

         

            Console.WriteLine("按做任意键关闭");
            Console.Read();

            Console.WriteLine("关闭中...");
     //       Server.Server.Stop();
            Console.WriteLine("关闭完成");

        }
    }
}
