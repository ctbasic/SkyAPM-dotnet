using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyApm.Soap.netcore;

namespace SkyApm.Sample.Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoapCallController : ControllerBase
    {
        // GET: api/SoapCall
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var instance = WebServiceFactory.GetInstance<skyapmsoap.SkyApmSoapServiceSoapClient>("http://localhost:57055/skyapmsoapservice.asmx");
            var a = instance.HelloWorldAsync().GetAwaiter().GetResult();
            return new string[] { "value1", "value2" };
        }

        // GET: api/SoapCall/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
    }
}
