using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SkyApm.Sample.Backend.thriftclient;

namespace SkyApm.Sample.Frontend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThriftCallController : ControllerBase
    {
        // GET: api/ThriftCall
        [HttpGet]
        [Route("test")]
        public IEnumerable<string> Test()
        {
            ThriftZipKinTest.Test();
            return new string[] { "value1", "value2" };
        }

    }
}
