using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using SkyApm.Agent.AspNet;

namespace SkyApm.Sample.AspNet45.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            //测试服务器
            string reqUrl = "http://192.168.1.201:3003/MvcDemo";
            var result = new ContentResult();

            //HttpClient httpClient = new HttpClient(new HttpTracingHandler());

            var httpClient = HttpClientFactory.Create(new HttpTracingHandler(null));

            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(reqUrl);

            var reqResult = httpClient.SendAsync(message).Result;

            result.Content = reqResult.Content.ReadAsStringAsync().Result;

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
