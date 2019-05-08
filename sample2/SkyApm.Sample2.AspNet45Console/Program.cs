using SkyApm.Agent.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SkyApm.Sample2.AspNet45Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CtSkyApmNetFxAgent.StartAsync();


            while (true)
            {
                try
                {

                    var httpClient = new HttpClient(new HttpTracingHandler());
                    HttpRequestMessage message = new HttpRequestMessage();
                    message.RequestUri = new Uri("http://192.168.1.201:5002/home/get");
                    var reqResult = httpClient.SendAsync(message).Result;
                    var data2 = reqResult.Content.ReadAsStringAsync().Result;

                    Console.WriteLine(data2);
                }
                catch { }

                Console.ReadLine();

            }



            Console.WriteLine("srtart..");
            Console.Read();
        }
    }
}
