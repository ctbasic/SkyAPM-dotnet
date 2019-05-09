﻿using System;
using System.Net;
using Log.TcySys.SDKEX;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SkyApm.Agent.NetCoreHost.Extensions;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using ThriftServer;

namespace SkyApm.Sample.ThriftBackend
{
    //public class mylogger : SkyApm.Logging.ILoggerFactory
    //{
    //    public mylogger()
    //    {
    //    }

    //    public SkyApm.Logging.ILogger CreateLogger(Type type)
    //    {
    //        return new myDefaultLogger();
    //    }

    //}

    //internal class myDefaultLogger : SkyApm.Logging.ILogger
    //{

    //    public myDefaultLogger()
    //    {
    //    }

    //    public void Debug(string message)
    //    {
    //        Console.WriteLine(message);
    //    }

    //    public void Information(string message)
    //    {
    //        Console.WriteLine(message);
    //    }

    //    public void Warning(string message)
    //    {
    //        Console.WriteLine(message);
    //    }

    //    public void Error(string message, Exception exception)
    //    {
    //        Console.WriteLine(message + Environment.NewLine + exception);
    //    }

    //    public void Trace(string message)
    //    {
    //        Console.WriteLine(message);
    //    }
    //}


    class Program
    {
        static void Main(string[] args)
        {
            IHostBuilder hostBuilder = new HostBuilder();
            hostBuilder.ConfigureLogging(

                logging => {  logging.SetMinimumLevel(LogLevel.Debug);
                       }
                
                )
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    #region  此部分内容必须置顶

                    IConfiguration config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables().Build();

                    hostBuilderContext.Configuration = config;

                    #endregion

                    services.AddSingleton<IHostedService, ThriftHostService>();
                    //services.AddSingleton<SkyApm.Logging.ILoggerFactory, mylogger>();


                    services.AddLogging(logBuilder =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        logBuilder.AddTcyLog(config, serviceProvider.GetService<IHttpContextAccessor>());
                    });
                    
                    })
                .AddSkyAPM()
                .Build().Run();



            //TServerSocket serverTransport = new TServerSocket(7911, 0, false);
            //HelloWorldService.Processor processor = new HelloWorldService.Processor(new Hello());
            //TServer server = new TThreadPoolServer(new TSingletonProcessorFactory(new BaseProcessor(processor)), serverTransport,
            //    new TTransportFactory(),
            //    new TTransportFactory(),
            //    new TCtSkyApmServerProtocol.Factory(),
            //    new TCtSkyApmServerProtocol.Factory(), 500, 1000, (x) =>
            //    {
            //        //ThriftLog.Info("log:" + x);
            //    });

            ////TServer server = new TThreadPoolServer(new TSingletonProcessorFactory(new BaseProcessor(processor)), serverTransport,
            ////    new TTransportFactory(),
            ////    new TTransportFactory(),
            ////    new TBinaryProtocolTest.Factory(),
            ////    new TBinaryProtocolTest.Factory(), 500, 1000, (x) =>
            ////    {
            ////        //ThriftLog.Info("log:" + x);
            ////    });
            //Console.WriteLine("Starting server on port 7911 ...");
            //server.Serve();
            //Console.ReadLine();
        }
    }

    public class BaseProcessor : TProcessor
    {
        private TProcessor _processor;

        public BaseProcessor(TProcessor processor)
        {
            _processor = processor;
        }

        /** 
         * 该方法，客户端每调用一次，就会触发一次 
         */
        public bool Process(TProtocol iprot, TProtocol oprot)
        {
            TSocket socket = (TSocket)iprot.Transport;

            IPEndPoint ip = (IPEndPoint)socket.TcpClient.Client.RemoteEndPoint;

            //string serviceName = "rpctest";
            //serverTrace = new ServerTrace(serviceName, ProtocolUtils.RPC_NAME);
            //string host = "127.0.0.1";
            //trace.Record(Annotations.Tag("http.host", host));
            //string url = methodName;
            //trace.Record(Annotations.Tag("http.uri", url));
            //string path = methodName;
            //trace.Record(Annotations.Tag("http.path", path));

            //serverTrace.TracedActionAsync(Task.Run(() => { })).Wait();



            return _processor.Process(iprot, oprot);
            //}

        }
    }
}
