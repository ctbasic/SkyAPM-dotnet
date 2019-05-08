using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thrift.ServiceWin_NETCore
{
    public class ThriftHostService : IHostedService
    {
        public ThriftHostService( )
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                //记录信息与错误日志
                Server.ThriftLog._eventInfo = (x) => { Console.WriteLine("info:" + x); };
                Server.ThriftLog._eventError = (x) => { Console.WriteLine("error:" + x); };

                Server.Server._funcTime = (methodName, parameters, elapsedMilliseconds) =>
                {
                    Console.WriteLine($"执行方法完成：{methodName}({Newtonsoft.Json.JsonConvert.SerializeObject(parameters)})  豪秒:{elapsedMilliseconds}");

                    //接入logsdk代码
                    //if (elapsedMilliseconds >= 2000)
                    //{
                    //    string msg = "";
                    //    if (parameters != null)
                    //        msg = JsonSerializer(parameters);
                    //    LogHelper.ApiUrlCall("xxxThrift:" + methodName, msg, elapsedMilliseconds, GetServerIP());
                    //}
                    //else
                    //    LogHelper.ApiUrlCall("xxxThrift:" + methodName, "",elapsedMilliseconds);
                };

                Server.Server._funcError = (methodName, parameters, exception) =>
                {
                    Console.WriteLine($"执行方法异常：{methodName}({Newtonsoft.Json.JsonConvert.SerializeObject(parameters)})  异常:{exception.Message}");

                    //接入logsdk代码
                    //    string msg = "";
                    //    if (parameters != null)
                    //        msg = JsonSerializer(parameters);
                    //LogHelper.ApiUrlException("xxxThrift:" + methodName, msg, exception);
                };

                Server.Server.Start();


            }, cancellationToken);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.Delay(10);
        }
    }
}
