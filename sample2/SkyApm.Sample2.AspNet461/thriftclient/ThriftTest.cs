using System;
using Thrift.Client;
using Thrift.Protocol;
using Thrift.Transport;

namespace SkyApm.Sample2.AspNet461.thriftclient
{
    public class ThriftTest
    {
        public static string Test()
        {
            using (var svc = ThriftClientManager<TcyApp.Demo.Thrift.ClassRoomThrift.Client>.GetClient("TcyAppDemoThrift"))
            {
                try
                {
                    var data = svc.Client.GetClassRoom();

                    return Newtonsoft.Json.JsonConvert.SerializeObject(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (svc != null)
                        svc.Destroy();

                    throw ex;
                }
            }

        }
    }
}
