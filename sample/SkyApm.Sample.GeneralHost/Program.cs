using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkyApm.Agent.GeneralHost;
using System.Net.Http;

namespace SkyApm.Sample.GeneralHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("start:" + System.DateTime.Now.ToString());

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseProxy = false;//不加这个会非常慢
            using (HttpClient httpclient = new HttpClient(handler))
            {
                while (true)
                {
                    var result = httpclient.GetStringAsync("http://localhost:5001/api/values").Result;
                    System.Threading.Thread.Sleep(100);
                }
            }


            System.Console.WriteLine("end:" + System.DateTime.Now.ToString());
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder()
                .ConfigureServices(services => services.AddHostedService<Worker>())
                .AddSkyAPM();
    }
}
