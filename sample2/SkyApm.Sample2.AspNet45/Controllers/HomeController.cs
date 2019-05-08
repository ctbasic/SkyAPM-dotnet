using SkyApm.Agent.AspNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SkyApm.Sample2.AspNet45.Controllers
{
    public class HomeController : Controller
    {
        public IEnumerable<string> Get()
        {
           var httpClient =  new HttpClient(new HttpTracingHandler());

            var values = httpClient.GetStringAsync("http://192.168.1.201:5001/Home/get").Result;

            return new string[] { "value1", "value2" };
        }

        public IEnumerable<string> Error()
        {
            throw new Exception("error test");
        }
    }
}