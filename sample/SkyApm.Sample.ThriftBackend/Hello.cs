using System;
using Microsoft.Extensions.DependencyInjection;
using SkyApm.CtCustom;
using SkyApm.Sample.ThriftBackend;

namespace ThriftServer
{
    class Hello:HelloWorldService.Iface
    {
        public void SayHello(string msgtest)
        {
            Console.WriteLine(msgtest);
        }

        public string GetString(string msgtest)
        {
            string traceId = DefaultServiceProvider.Instance.GetService<ICustomSegmentContextAccessor>().CurrentTraceId;

            return msgtest + "OK"+ traceId;
        }

        public UserInfo GetUserInfo(int userId, string userName)
        {
            return new UserInfo(){UserID = 111,UserName = "JACK"};
        }
    }
}
