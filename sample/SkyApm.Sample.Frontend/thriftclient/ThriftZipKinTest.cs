using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace SkyApm.Sample.Backend.thriftclient
{
    public class ThriftZipKinTest
    {
        public static void Test()
        {
            TTransport transport = new TSocket("localhost", 7911);
            TProtocol protocol = new TCtSkyApmClientBinaryProtocol(transport);
            HelloWorldService.Client client = new HelloWorldService.Client(protocol);
            transport.Open();
            Console.WriteLine("Client calls .....");
            //client.SayHello("asdasdasd");
            //client.GetString("OK");

            client.GetUserInfo(1, "HHH-jack");
            transport.Close();
        }
    }
}
