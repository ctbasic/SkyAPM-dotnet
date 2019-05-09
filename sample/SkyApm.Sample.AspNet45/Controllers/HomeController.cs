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
    public class HomeController : Controller
    {
         
        // GET api/values
        public IEnumerable<string> Get()
        {
            var httpClient = HttpClientFactory.Create(new HttpTracingHandler(null));

            var values =  httpClient.GetStringAsync("http://localhost:5001/api/values").Result;

            return new string[] { "value1", "value2" };
        }

        public IEnumerable<string> Error()
        {
            throw new Exception("error te333st");
        }

    }
}
