#if NETSTANDARD
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace SkyApm.Soap.netcore
{
    /// <summary>
    /// 基础服务信息
    /// </summary>
    [Obsolete]
    public static class WebServiceFactory
    {
        #region Private Properties

        private static CommunicationState[] needRemoveCommunicationState = { CommunicationState.Closed, CommunicationState.Closing, CommunicationState.Faulted };
        private static ConcurrentDictionary<string, object> concurrentDictionary = new ConcurrentDictionary<string, object>();
        private static object lockObject = new object();

        #endregion

        /// <summary>
        /// 获取服务实例信息
        /// </summary>
        /// <returns></returns>
        public static TServiceClass GetInstance<TServiceClass>(string serviceUrl) where TServiceClass : ICommunicationObject
        {
            if (string.IsNullOrEmpty(serviceUrl))
            {
                return default(TServiceClass);
            }
            if (concurrentDictionary.TryGetValue(serviceUrl, out object value) && CheckSoapStatus(serviceUrl, value))
            {
                return (TServiceClass)value;
            }

            lock (lockObject)
            {
                if (concurrentDictionary.TryGetValue(serviceUrl, out value) && CheckSoapStatus(serviceUrl, value))
                {
                    return (TServiceClass)value;
                }

                var createInstance = CreateServiceClass<TServiceClass>(serviceUrl);
                concurrentDictionary.TryAdd(serviceUrl, createInstance);
                return createInstance;
            }
        }

        /// <summary>
        /// 创建服务对象
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <returns></returns>
        private static ServiceClass CreateServiceClass<ServiceClass>(string serviceUrl)
        {
            var address = new EndpointAddress(serviceUrl);
            Binding binding;
            if (serviceUrl.StartsWith("https://"))
            {
                binding = GetHttpsBindingForEndpoint();
            }
            else
            {
                binding = GetBindingForEndpoint(EndpointConfiguration.ServiceSoap);
            }

            var createInstance = default(ServiceClass);
            var type = typeof(ServiceClass);
            if (type.IsInterface)
            {
                createInstance = new ChannelFactory<ServiceClass>(binding, address).CreateChannel();
            }
            else
            {
                createInstance = (ServiceClass)Activator.CreateInstance(typeof(ServiceClass), binding, address);
            }
            // 设置skyapmclient
            System.Reflection.PropertyInfo property = createInstance.GetType().GetProperty("Endpoint");
            ServiceEndpoint endpoint = (ServiceEndpoint)property.GetValue(createInstance, null);
            //endpoint.SetSkyApmClientBehavior();
            return createInstance;
        }

        /// <summary>
        /// 检查实例状态
        /// </summary>
        /// <param name="value"></param>
        private static bool CheckSoapStatus(string serviceUrl, object value)
        {
            var client = (ICommunicationObject)value;
            if (needRemoveCommunicationState.Contains(client.State))
            {
                concurrentDictionary.TryRemove(serviceUrl, out value);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取绑定信息
        /// </summary>
        /// <returns></returns>
        private static Binding GetHttpsBindingForEndpoint()
        {
            System.ServiceModel.BasicHttpsBinding result = new System.ServiceModel.BasicHttpsBinding(BasicHttpsSecurityMode.Transport);
            result.MaxBufferSize = int.MaxValue;
            result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            result.MaxReceivedMessageSize = int.MaxValue;
            result.AllowCookies = true;
            return result;
        }

        /// <summary>
        /// 获取绑定信息
        /// </summary>
        /// <param name="endpointConfiguration"></param>
        /// <returns></returns>
        private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.ServiceSoap))
            {
                System.ServiceModel.BasicHttpBinding result = new System.ServiceModel.BasicHttpBinding();
                result.MaxBufferSize = int.MaxValue;
                result.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
                result.MaxReceivedMessageSize = int.MaxValue;
                result.AllowCookies = true;
                return result;
            }
            if ((endpointConfiguration == EndpointConfiguration.ServiceSoap12))
            {
                System.ServiceModel.Channels.CustomBinding result = new System.ServiceModel.Channels.CustomBinding();
                System.ServiceModel.Channels.TextMessageEncodingBindingElement textBindingElement = new System.ServiceModel.Channels.TextMessageEncodingBindingElement();
                textBindingElement.MessageVersion = System.ServiceModel.Channels.MessageVersion.CreateVersion(System.ServiceModel.EnvelopeVersion.Soap12, System.ServiceModel.Channels.AddressingVersion.None);
                result.Elements.Add(textBindingElement);
                System.ServiceModel.Channels.HttpTransportBindingElement httpBindingElement = new System.ServiceModel.Channels.HttpTransportBindingElement();
                httpBindingElement.AllowCookies = true;
                httpBindingElement.MaxBufferSize = int.MaxValue;
                httpBindingElement.MaxReceivedMessageSize = int.MaxValue;
                result.Elements.Add(httpBindingElement);
                return result;
            }

            throw new System.InvalidOperationException(string.Format("Could not find endpoint with name \'{0}\'.", endpointConfiguration));
        }

        /// <summary>
        /// 终端配置信息
        /// </summary>
        private enum EndpointConfiguration
        {
            ServiceSoap,

            ServiceSoap12,
        }
    }
}
#endif