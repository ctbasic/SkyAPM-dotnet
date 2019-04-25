using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SkyApm.Sample.AspNet461.soapapmservice;

namespace SkyApm.Sample.AspNet461.Controllers
{
    public class DefaultController : ApiController
    {
        // GET: api/Default
        public IEnumerable<string> Get()
        {

            SkyApmSoapService client = new SkyApmSoapService();
            client.HelloWorld();
            return new string[] { "value1", "value2" };
        }
    }
}
