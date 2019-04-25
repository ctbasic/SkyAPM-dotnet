#if NETSTANDARD
using System.ServiceModel.Description;

namespace SkyApm.Soap.netcore
{
    public static class ServiceEndpointSkyApmExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="endpoint"></param>
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
