using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkyApm.Soap.netcore;
using Thrift.Client;

namespace SkyApm.Sample2.NetCoreWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var httpClient = HttpClientFactory.Create();
            var result = httpClient.GetStringAsync("http://192.168.1.201:5002/Home/get").Result;


            var instance = WebServiceFactory.GetInstance<skyapmsoap.SkyApmSoapServiceSoapClient>("http://192.168.1.201:5001/skyapmsoapservice.asmx");
            var result2 = instance.HelloWorldAsync().GetAwaiter().GetResult();


            string result3 = "";
            using (var svc = ThriftClientManager<TcyApp.Demo.Thrift.ClassRoomThrift.Client>.GetClient("TcyAppDemoThrift"))
            {
                {
                    try
                    {
                        var info = svc.Client.GetClassRoom();
                        result3=Newtonsoft.Json.JsonConvert.SerializeObject(info);
                    }
                    catch (Exception ex)
                    {
                        result3=ex.Message;
                        if (svc != null)
                            svc.Destroy();
                    }
                }
            }

            return new string[] { result, result2.Body.HelloWorldResult, result3 };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
