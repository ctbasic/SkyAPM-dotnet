using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SkyApm.Sample.Frontend.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static HttpClient client = new HttpClient();
        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseProxy = false;//不加这个会非常慢
            using (HttpClient httpclient = new HttpClient(handler))
            {
                  var result = httpclient.GetStringAsync("http://192.168.1.201:5002/api/default").Result;
            }
            return new string[] {"value1", "val  due2"};
        }

        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            var client = new HttpClient();
            Task.WhenAll(client.GetAsync("http://localhost:5003/api/delay/2000"),
                client.GetAsync("http://localhost:5003/api/values"),
                client.GetAsync("http://localhost:5003/api/delay/200"));
            return await client.GetStringAsync("http://localhost:5003/api/delay/100");
        }
    }
}