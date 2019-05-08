#if NETCORE
using Microsoft.Extensions.Configuration;
#endif
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Server;
using Thrift.Server.Config;
using Thrift.Transport;

namespace Thrift.Server
{
    public class Server
    {
        /// <summary>
        /// 方法执行时间
        /// </summary>
        public static Action<string, object[], long> _funcTime = null;

        /// <summary>
        /// 方法执行错误
        /// </summary>
        public static Action<string, object[], Exception> _funcError = null;

        private static Dictionary<TServer, RegeditConfig> _services = new Dictionary<TServer, RegeditConfig>();
        private const int _defaultDelayedTime = 20000; //默认延时关闭时间

#if NETCORE

        public static void Instance(IConfiguration configuration)
        {
            Config.Config.Instance(configuration);
        }

#endif

        public static void Start()
        {
#if NETCORE
            var config = Config.Config.GetServices();

#else
            var _configPath = ConfigurationManager.AppSettings["ThriftServerConfigPath"];
            Config.ThriftConfigSection config = null;
            if (string.IsNullOrEmpty(_configPath))
                config = ConfigurationManager.GetSection("thriftServer") as Config.ThriftConfigSection;
            else
            {
                config = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
                {
                    ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _configPath)
                }, ConfigurationUserLevel.None).GetSection("thriftServer") as Config.ThriftConfigSection;
            }
#endif

            if (config == null || config.Services == null)
                throw new Exception("thrift服务配置不存在");

                foreach (Service service in config.Services)
            {
                new System.Threading.Thread(() =>
                {
                    try
                    {
                        TProcessor processor = null;

                        if (service.IsMult)
                        {
                            processor = GetProcessorByAssemblyMult(service);
                        }
                        else
                        {
#if NETCORE
                            processor = GetProcessorByAssembly(service);
#else
                            //老的配置模式
                            if (!string.IsNullOrEmpty(service.HandlerType))
                                processor = GetProcessorByHandlerType(service);
                            else
                                processor = GetProcessorByAssembly(service);
#endif
                        }

                        if (service.Port > 0)
                        {
                            //if (!PortHelper.PortIsAvailable(service.Port))
                            //    throw new Exception("端口冲突");
                        }
                        else
                        {
                            service.Port = PortHelper.GetFirstAvailablePort();

                            if (service.Port <= 0)
                                throw new Exception("无端口可用");
                        }

                        TProtocolFactory inputProtocolFactory = null;
                        TProtocolFactory outputProtocolFactory = null;
                        if (service.ZipKin)
                        {
                            //inputProtocolFactory = new  TBinaryProtocol.Factory();
                            //outputProtocolFactory = new TBinaryProtocol.Factory();

                            inputProtocolFactory = new TCtSkyApmServerProtocol.Factory();
                            outputProtocolFactory = new TCtSkyApmServerProtocol.Factory();
                        }
                        else
                        {
                            //inputProtocolFactory = new TBinaryProtocol.Factory();
                            //outputProtocolFactory = new TBinaryProtocol.Factory();

                            inputProtocolFactory = new TCtSkyApmServerProtocol.Factory();
                            outputProtocolFactory = new TCtSkyApmServerProtocol.Factory();
                        }



                        TServerTransport serverTransport = new TServerSocket(service.Port, service.ClientTimeout);

                        TServer server = new TThreadPoolServer(new TSingletonProcessorFactory(new BaseProcessor(processor, service)), serverTransport,
                            new TTransportFactory(),
                            new TTransportFactory(),
                           inputProtocolFactory,
                          outputProtocolFactory, service.MinThreadPoolThreads, service.MaxThreadPoolThreads, (x) =>
                            {
                                ThriftLog.Info("log:" + x);
                            });

                        RegeditConfig regiditConfig = null;
                        if (service.ZookeeperConfig != null && service.ZookeeperConfig.Host != "")
                            regiditConfig = ConfigCenter.RegeditServer(service); //zookeeper 注册服务

                        ThriftLog.Info($"{service.Name} {service.Port} Starting the TThreadPoolServer...");
                        _services.Add(server, regiditConfig);
                        server.Serve();
                    }
                    catch (Exception ex)
                    {
                        ThriftLog.Error(ex.Message + ex.StackTrace);
                    }
                }).Start();
            }
        }

#if NETCORE

        private static TProcessor GetProcessorByAssembly(Service service)
        {
            Type[] thriftTypes = Assembly.Load(service.ThriftAssembly).GetTypes();
            Type[] thriftImplTypes = Assembly.Load(service.ThriftImplAssembly).GetTypes();

            foreach (var t in thriftTypes)
            {
                if (!t.Name.Equals("Iface"))
                {
                    continue;
                }
                string processorFullName = t.FullName.Replace("+Iface", "+Processor");
                Type processorType = thriftTypes.FirstOrDefault(c => c.FullName.Equals(processorFullName));
                object handle = null;
                foreach (Type t2 in thriftImplTypes)
                {
                    if (t2.GetInterfaces().Contains(t))
                    {
                        var TProxy = typeof(InvokeProxy<>).MakeGenericType(t2);

                        handle = TransparentProxy.Create(TProxy, t);
                        return (Thrift.TProcessor)processorType.GetConstructor(new Type[] { t }).Invoke(new object[] { handle });
                    }
                }
            }
            return null;
        }

        private static TProcessor GetProcessorByAssemblyMult(Service service)
        {
            TMultiplexedProcessor multiplexedProcessor = new TMultiplexedProcessor();

            Type[] thriftTypes = Assembly.Load(service.ThriftAssembly).GetTypes();
            Type[] thriftImplTypes = Assembly.Load(service.ThriftImplAssembly).GetTypes();


            foreach (var t in thriftTypes)
            {
                if (!t.Name.Equals("Iface"))
                {
                    continue;
                }
                string processorFullName = t.FullName.Replace("+Iface", "+Processor");
                Type processorType = thriftTypes.FirstOrDefault(c => c.FullName.Equals(processorFullName));
                object handle = null;
                foreach (Type t2 in thriftImplTypes)
                {
                    if (t2.GetInterfaces().Contains(t))
                    {
                        var TProxy = typeof(InvokeProxy<>).MakeGenericType(t2);
                        handle = TransparentProxy.Create(TProxy, t);
                        break;
                    }
                }
                var processor = (Thrift.TProcessor)processorType.GetConstructor(new Type[] { t }).Invoke(new object[] { handle });
                multiplexedProcessor.RegisterProcessor(t.ReflectedType.Name, processor);
            }
            return multiplexedProcessor;
        }


#else
        private static TProcessor GetProcessorByHandlerType(Service service)
        {
            Assembly assembly = Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, service.HandlerType.Split(',')[1]));
            object objType = assembly.CreateInstance(service.HandlerType.Split(',')[0], true);
            if (objType == null)
                throw new Exception(service.HandlerType + "为空");

            var handle = TransparentProxy.Create(objType.GetType());

            return (Thrift.TProcessor)Type.GetType($"{service.SpaceName}.{service.ClassName}+Processor,{service.SpaceName}", true)
          .GetConstructor(new Type[] { Type.GetType($"{service.SpaceName}.{service.ClassName}+Iface,{service.SpaceName}", true) })
             .Invoke(new object[] { handle });
        }

        private static TProcessor GetProcessorByAssembly(Service service)
        {
            Type[] thriftTypes = Assembly.Load(service.ThriftAssembly).GetTypes();
            Type[] thriftImplTypes = Assembly.Load(service.ThriftImplAssembly).GetTypes();

            foreach (var t in thriftTypes)
            {
                if (!t.Name.Equals("Iface"))
                {
                    continue;
                }
                string processorFullName = t.FullName.Replace("+Iface", "+Processor");
                Type processorType = thriftTypes.FirstOrDefault(c => c.FullName.Equals(processorFullName));
                object handle = null;
                foreach (Type t2 in thriftImplTypes)
                {
                    if (t2.GetInterfaces().Contains(t))
                    {
                        handle = TransparentProxy.Create(t2);
                        return (Thrift.TProcessor)processorType.GetConstructor(new Type[] { t }).Invoke(new object[] { handle });
                    }
                }
            }
            return null;
        }

        private static TProcessor GetProcessorByAssemblyMult(Service service)
        {
            TMultiplexedProcessor multiplexedProcessor = new TMultiplexedProcessor();

            Type[] thriftTypes = Assembly.Load(service.ThriftAssembly).GetTypes();
            Type[] thriftImplTypes = Assembly.Load(service.ThriftImplAssembly).GetTypes();
            foreach (var t in thriftTypes)
            {
                if (!t.Name.Equals("Iface"))
                {
                    continue;
                }
                string processorFullName = t.FullName.Replace("+Iface", "+Processor");
                Type processorType = thriftTypes.FirstOrDefault(c => c.FullName.Equals(processorFullName));
                object handle = null;
                foreach (Type t2 in thriftImplTypes)
                {
                    if (t2.GetInterfaces().Contains(t))
                    {
                        handle = TransparentProxy.Create(t2);
                        break;
                    }
                }
                var processor = (Thrift.TProcessor)processorType.GetConstructor(new Type[] { t }).Invoke(new object[] { handle });
                multiplexedProcessor.RegisterProcessor(t.ReflectedType.Name, processor);
            }
            return multiplexedProcessor;
        }

#endif
        public static void Stop()
        {
            //先注销zookeeper
            try
            {
                foreach (var server in _services)
                {
                    if (server.Value != null)
                        server.Value.Logout();
                }
            }
            catch (Exception ex)
            {
                ThriftLog.Error(ex.Message);
            }

            if (_services.Count > 0)
            {
#if NETCORE
                System.Threading.Thread.Sleep(Config.Config.GetThriftServerStopDelayedTime()); //延时关闭
#else
                int delayedTime = _defaultDelayedTime;
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ThriftServerStopDelayedTime"]))
                    delayedTime = int.Parse(ConfigurationManager.AppSettings["ThriftServerStopDelayedTime"]);

                System.Threading.Thread.Sleep(delayedTime); //延时关闭
#endif
            }

            //再关闭服务
            foreach (var server in _services)
            {
                if (server.Key != null)
                    server.Key.Stop();
            }

            _services.Clear();
        }
    }
}
