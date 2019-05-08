﻿using System.Collections.Generic;
using System.Configuration;

namespace Thrift.Server.Config
{
#if NETCORE
    public class ThriftConfigSection
    {
        public List<Service> Services { get; set; }
    }
#else
    /// <summary>
    /// thrift config section
    /// </summary>
    public class ThriftConfigSection : ConfigurationSection
    {
        /// <summary>
        /// 服务集合。
        /// </summary>
        [ConfigurationProperty("services", IsRequired = true)]
        public ServiceCollection Services
        {
            get { return this["services"] as ServiceCollection; }
        }
    }
#endif
}