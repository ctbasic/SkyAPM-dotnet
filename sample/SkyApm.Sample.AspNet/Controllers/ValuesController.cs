using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SkyApm.Agent.AspNet;

namespace SkyApm.Sample.AspNet.Controllers
{
    public class ValuesController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            //var httpClient = new HttpClient(new HttpTracingHandler());

            var httpClient = HttpClientFactory.Create(new HttpTracingHandler(null));

            //var values = await httpClient.GetStringAsync("http://localhost:5002/api/values");

            var values = await httpClient.GetStringAsync("http://192.168.1.201:3003/MvcDemo");
            return Ok();
        }
    }
}