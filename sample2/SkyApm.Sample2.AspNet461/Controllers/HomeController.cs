using SkyApm.Agent.AspNet;
using SkyApm.Sample2.AspNet461.thriftclient;
using SkyApm.Sample2.AspNet461.WebReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SkyApm.Sample2.AspNet461.Controllers
{
    public class HomeController : Controller
    {
        public string Get()
        {
            SkyApmSoapService client = new SkyApmSoapService();
            var data = client.HelloWorld();


            //  var httpClient = HttpClientFactory.Create(new HttpTracingHandler(null));
            var httpClient = new HttpClient(new HttpTracingHandler());
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
                data3 = "error";
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
            catch (Exception ex){

                data4 = ex.Message;
            }


            return             Newtonsoft.Json.JsonConvert.SerializeObject(new string[] { data, data2, data3, data4 });

        }


    }
}