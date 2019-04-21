using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;
using ThriftServer;

namespace SkyApm.Sample.ThriftBackend
{
    public class ThriftHostService: IHostedService
    {

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                TServerSocket serverTransport = new TServerSocket(7911, 0, false);
                HelloWorldService.Processor processor = new HelloWorldService.Processor(new Hello());
                TServer server = new TThreadPoolServer(new TSingletonProcessorFactory(new Program.BaseProcessor(processor)), serverTransport,
                    new TTransportFactory(),
                    new TTransportFactory(),
                    new TCtSkyApmServerProtocol.Factory(),
                    new TCtSkyApmServerProtocol.Factory(), 500, 1000, (x) =>
                    {
                        //ThriftLog.Info("log:" + x);
                    });

                Console.WriteLine("Starting server on port 7911 ...");
                server.Serve();
            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Delay(10);
        }
    }
}
