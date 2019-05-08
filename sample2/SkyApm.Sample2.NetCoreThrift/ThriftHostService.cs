using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Thrift;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Transport;


namespace SkyApm.Sample2.NetCoreThrift
{
    public class ThriftHostService : IHostedService
    {
        public ThriftHostService()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                //记录信息与错误日志
                ThriftLog._eventInfo = (x) => { Console.WriteLine("info:" + x); };
                ThriftLog._eventError = (x) => { Console.WriteLine("error:" + x); };

                Server.Start();
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Delay(10);
        }
    }
}
