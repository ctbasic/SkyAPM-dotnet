#if NETSTANDARD
using System;
using System.ServiceModel.Description;

namespace SkyApm.Soap.netcore
{

    [Obsolete]
    public static class ServiceEndpointSkyApmExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
        [Obsolete]
        public static void SetSkyApmClientBehavior(this ServiceEndpoint endpoint)
        {
            if (!endpoint.EndpointBehaviors.Contains(typeof(SkyApmClientBehavior)))
            {
                endpoint.EndpointBehaviors.Add(new SkyApmClientBehavior());
            }
        }
    }
}
#endif
