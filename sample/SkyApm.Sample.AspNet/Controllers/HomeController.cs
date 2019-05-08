//using SkyApm.Agent.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SkyApm.Sample.AspNet45.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return new ContentResult() { Content = "aspnet45 mvc test" };
        }

        // GET api/values
        //public string GetApi(string reqUrl)
        //{
        //    var result = new ContentResult();

        //    //HttpClient httpClient = new HttpClient(new HttpTracingHandler());

        //    //var httpClient = HttpClientFactory.Create(new HttpTracingHandler(null));

        //    HttpRequestMessage message = new HttpRequestMessage();
        //    message.RequestUri = new Uri(reqUrl);

        //    var reqResult = httpClient.SendAsync(message).Result;

        //    result.Content = reqResult.Content.ReadAsStringAsync().Result;

        //    return result.Content;
        //}

        // GET api/values/5
        public string GetThrift()
        {
            return "value";
        }
    }
}
