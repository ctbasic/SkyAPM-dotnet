using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SkyApm.Agent.AspNet;
using SkyApm.Sample.AspNet461.soapapmservice;
using SkyApm.Sample.AspNet461.thriftclient;

namespace SkyApm.Sample.AspNet461.Controllers
{
    public class DefaultController : ApiController
    {
        // GET: api/Default
        public IEnumerable<string> Get()
        {
            string currentTranceId = CtSkyApmNetFxAgent.CurrentTranceId;

            SkyApmSoapService client = new SkyApmSoapService();
            var data = client.HelloWorld();


            var httpClient = new HttpClient(new HttpTracingHandler());
            // var httpClient = HttpClientFactory.Create(new HttpTracingHandler(null));
            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri("http://192.168.1.201:5001/Home/get");
            var reqResult = httpClient.SendAsync(message).Result;
            var data2 = reqResult.Content.ReadAsStringAsync().Result;


            var data3 = "";
            try
            {
                httpClient = new HttpClient(new HttpTracingHandler());
                message = new HttpRequestMessage();
                message.RequestUri = new Uri("http://192.168.1.201:5001/Home/Error");
                reqResult = httpClient.SendAsync(message).Result;
                data3 = reqResult.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                data3 = ex.Message;
            }

            var data4 = "";

            try
            {
                data4 = ThriftTest.Test();
            }
            catch { }
            return new string[] { data, data2, data3, data4 };
        }
    }


}
