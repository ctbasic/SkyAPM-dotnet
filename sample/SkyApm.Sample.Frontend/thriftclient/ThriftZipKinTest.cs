using System;
using Thrift.Protocol;
using Thrift.Transport;

namespace SkyApm.Sample.Backend.thriftclient
{
    public class ThriftZipKinTest
    {
        //private static HelloWorldService.Client client;
        static ThriftZipKinTest()
        {
            //TTransport transport = new TSocket("localhost", 7911);
            //TProtocol protocol = new TCtSkyApmClientBinaryProtocol(transport);
            //client = new HelloWorldService.Client(protocol);
            //transport.Open();
        }

        public static void Test()
        {
            TTransport transport = new TSocket("localhost", 7911);
            TProtocol protocol = new TCtSkyApmClientBinaryProtocol(transport);
           var client = new HelloWorldService.Client(protocol);
            transport.Open();

            Console.WriteLine("Client calls .....");
            client.SayHello("asdasdasd");
            //client.GetString("OK");

            //client.GetUserInfo(1, "HHH-jack");
            transport.Close();
        }
    }
}
