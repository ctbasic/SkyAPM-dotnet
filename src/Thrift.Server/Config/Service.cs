using System;
using System.Configuration;

namespace Thrift.Server.Config
{
#if NETCORE
    /// <summary>
    /// service config
    /// </summary>
    public class Service
    {
        /// <summary>
        /// 是否启用zipkin
        /// </summary>
        public bool ZipKin
        {
            get; set;
        } = false;

        /// <summary>
        /// 服务名称
        /// </summary>
        public string Name
        {
            get; set;
        } = "";

        /// <summary>
        /// 是否多服务
        /// </summary>
        public bool IsMult
        {
            get; set;
        } = false;

        /// <summary>
        /// thrift程序集
        /// </summary>
        public string ThriftAssembly
        {
            get; set;
        } = "";

        /// <summary>
        /// Thrift实现的程序集
        /// </summary>
        public string ThriftImplAssembly
        {
            get; set;
        } = "";


        /// <summary>
        /// 最小连接数
        /// </summary>
        public int MinThreadPoolThreads
        {
            get; set;
        } = 500;

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int MaxThreadPoolThreads
        {
            get; set;
        } = 1000;

        /// <summary>
        /// 客户端连接超时
        /// </summary>
        public int ClientTimeout
        {
            get; set;
        } = 0;

        /// <summary>
        /// host
        /// </summary>
        public string Host
        {
            get; set;
        } = "";

        /// <summary>
        /// Port
        /// </summary>
        public int Port
        {
            get; set;
        }

        /// <summary>
        /// 服务器权重
        /// </summary>
        public int Weight
        {
            get; set;
        } = 1;

        /// <summary>
        /// zookeeper 配置 获取地址与端口号
        /// </summary>
        public ZookeeperConfig ZookeeperConfig
        {
            get; set;
        }
    }
#else
    /// <summary>
    /// service config
    /// </summary>
    public class Service : ConfigurationElement
    {
        /// <summary>
        /// 是否启用zipkin
        /// </summary>
        [ConfigurationProperty("zipkin", IsRequired = false, DefaultValue = false)]
        public bool ZipKin
        {
            get; set;
        } = false;

        /// <summary>
        /// 服务名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
        }

        /// <summary>
        /// 是否多服务
        /// </summary>
        [ConfigurationProperty("isMult", IsRequired = false, DefaultValue = false)]
        public bool IsMult
        {
            get { return (bool)this["isMult"]; }
        }

        /// <summary>
        /// thrift程序集
        /// </summary>
        [ConfigurationProperty("thriftAssembly", IsRequired = false, DefaultValue = "")]
        public string ThriftAssembly
        {
            get { return (string)this["thriftAssembly"]; }
        }

        /// <summary>
        /// Thrift实现的程序集
        /// </summary>
        [ConfigurationProperty("thriftImplAssembly", IsRequired = false, DefaultValue = "")]
        public string ThriftImplAssembly
        {
            get { return (string)this["thriftImplAssembly"]; }
        }

        /// <summary>
        /// 实现dll 兼容老版本
        /// </summary>
        [ConfigurationProperty("handlerType", IsRequired = false, DefaultValue = "")]
        public string HandlerType
        {
            get { return (string)this["handlerType"]; }
        }

        /// <summary>
        /// 命名空间名称 兼容老版本
        /// </summary>
        [ConfigurationProperty("spaceName", IsRequired = false, DefaultValue = "")]
        public string SpaceName
        {
            get { return (string)this["spaceName"]; }
        }

        /// <summary>
        /// 实现类名称 兼容老版本
        /// </summary>
        [ConfigurationProperty("className", IsRequired = false, DefaultValue = "")]
        public string ClassName
        {
            get { return (string)this["className"]; }
        }

        /// <summary>
        /// 最小连接数
        /// </summary>
        [ConfigurationProperty("minThreadPoolThreads", IsRequired = false, DefaultValue = 500)]
        public int MinThreadPoolThreads
        {
            get { return (int)this["minThreadPoolThreads"]; }
        }

        /// <summary>
        /// 最大连接数
        /// </summary>
        [ConfigurationProperty("maxThreadPoolThreads", IsRequired = false, DefaultValue =1000)]
        public int MaxThreadPoolThreads
        {
            get { return (int)this["maxThreadPoolThreads"]; }
        }

        /// <summary>
        /// 客户端连接超时
        /// </summary>
        [ConfigurationProperty("clientTimeout", IsRequired = false, DefaultValue = 0)]
        public int ClientTimeout
        {
            get { return (int)(this["clientTimeout"]); }
        }

        /// <summary>
        /// host
        /// </summary>
        [ConfigurationProperty("host", IsRequired = false, DefaultValue = "")]
        public string Host
        {
            get { return (string)this["host"]; }
        }

        /// <summary>
        /// Port
        /// </summary>
        [ConfigurationProperty("port", IsRequired = false, DefaultValue = 0)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        /// <summary>
        /// 服务器权重
        /// </summary>
        [ConfigurationProperty("weight", IsRequired = false, DefaultValue = 1)]
        public int Weight
        {
            get { return (int)this["weight"]; }
        }

        /// <summary>
        /// zookeeper 配置 获取地址与端口号
        /// </summary>
        [ConfigurationProperty("ZookeeperConfig", IsRequired = false, DefaultValue = null)]
        public ZookeeperConfig ZookeeperConfig
        {
            get { return this["ZookeeperConfig"] as ZookeeperConfig; }
        }
    }
#endif
}