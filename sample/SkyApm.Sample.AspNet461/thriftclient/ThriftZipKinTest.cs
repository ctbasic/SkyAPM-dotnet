using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace SkyApm.Sample.AspNet461.thriftclient
{
    public class ThriftTest
    {
        //private static HelloWorldService.Client client;
        static ThriftTest()
        {
            //TTransport transport = new TSocket("localhost", 7911);
            //TProtocol protocol = new TCtSkyApmClientBinaryProtocol(transport);
            //client = new HelloWorldService.Client(protocol);
            //transport.Open();
        }

        public static string Test()
        {
            TTransport transport = new TSocket("localhost", 7911);
            TProtocol protocol = new TCtSkyApmClientBinaryProtocol(transport);
           var client = new HelloWorldService.Client(protocol);
            transport.Open();

            Console.WriteLine("Client calls .....");
           // client.SayHello("asdasdasd");
          var data=  client.GetString("OK");

            //client.GetUserInfo(1, "HHH-jack");
            transport.Close();
            return data;
        }
    }
}
