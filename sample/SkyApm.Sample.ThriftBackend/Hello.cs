using System;

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
            return msgtest + "OK";
        }

        public UserInfo GetUserInfo(int userId, string userName)
        {
            return new UserInfo(){UserID = 111,UserName = "JACK"};
        }
    }
}
