using System;
using System.Configuration;

namespace SkyApm.Sample.AspNet461
{
    /// <summary>
    /// Web服务基类，支持配置或动态多域名判定
    /// </summary>
    /// <typeparam name="S"></typeparam>
    public class BaseWebService<S> where S : System.Web.Services.Protocols.SoapHttpClientProtocol, new()
    {
        /// <summary>
        /// 服务实例
        /// </summary>
        private static S serviceInstance = null;

        /// <summary>
        /// 静态锁
        /// </summary>
        private static object lockHelper = new object();

        /// <summary>
        /// 获取服务实例对象
        /// </summary>
        /// <returns>服务实例对象</returns>
        protected static S GetInstance()
        {
            if (serviceInstance != null)
            {
                return serviceInstance;
            }
            lock (lockHelper)
            {
                if (serviceInstance == null)
                {
                    try
                    {
                        serviceInstance = new S();
                        serviceInstance.Url = "http://localhost:57055/skyapmsoapservice.asmx";
                    }
                    catch (Exception ex)
                    {
                        serviceInstance = null;
                        throw ex;
                    }
                }
            }
            return serviceInstance;
        }
    }
}
