#if NETCORE
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Thrift.Server.Config
{
    internal class Config
    {
        private static int _thriftServerStopDelayedTime; //默认延时关闭时间
        private static string _thriftServerConfigPath;
        private static string _thriftIpLimitPath;
        private static IConfigurationRoot _configurationRoot;

        public static int GetThriftServerStopDelayedTime()
        {
            return _thriftServerStopDelayedTime;
        }

        public static void Instance(IConfiguration configuration)
        {
            _thriftServerConfigPath = configuration["ThriftServer:ConfigPath"];
            _thriftIpLimitPath = configuration["ThriftServer:IpLimitPath"];

            _configurationRoot = new ConfigurationBuilder()
.AddJsonFile(Path.Combine(AppContext.BaseDirectory, _thriftIpLimitPath), optional: false, reloadOnChange: true)
.Build();

            if (configuration["ThriftServer:StopDelayedTime"] != null)
                _thriftServerStopDelayedTime = int.Parse(configuration["ThriftServer:StopDelayedTime"]);
            else
                _thriftServerStopDelayedTime = 20000;
        }

        public static ThriftConfigSection GetServices()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
       .AddJsonFile(Path.Combine(AppContext.BaseDirectory, _thriftServerConfigPath), optional: false, reloadOnChange: true)
       .Build();

            return config.Get<ThriftConfigSection>();
        }

        #region GetIpLimit

        private static string _ipLimitHashCode = "";
        private static List<Tuple<long, long>> _listIpLimit = null;

        public static List<Tuple<long, long>> GetIpLimit()
        {
            var ipLimitInfo = _configurationRoot.Get<IpLimitInfo>();
            if (ipLimitInfo == null) return null;

            var newIpLimitHashCode = Newtonsoft.Json.JsonConvert.SerializeObject(ipLimitInfo.IpWhite);

            if (_ipLimitHashCode == newIpLimitHashCode && _listIpLimit != null)
                return _listIpLimit;

            _ipLimitHashCode = newIpLimitHashCode;
            _listIpLimit = GetConfig_Ranges(ipLimitInfo.IpWhite);
            return _listIpLimit;
        }

        private static List<Tuple<long, long>> GetConfig_Ranges(string[] strRanges)
        {

            List<Tuple<long, long>> listConfig = new List<Tuple<long, long>>();

            foreach (string range in strRanges)
                listConfig.Add(GenerateIpRange(range));

            return listConfig;
        }

        private static Tuple<long, long> GenerateIpRange(string range)
        {
            var ipArray = range.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

            if (ipArray.Length == 0)
                throw new ArgumentException("IP限制配置错误!");

            if (ipArray.Length == 1)
                return new Tuple<long, long>(IPToLong(ipArray[0]), IPToLong(ipArray[0]));
            else
                return new Tuple<long, long>(IPToLong(ipArray[0]), IPToLong(ipArray[1]));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ip">IP字符串</param>
        /// <returns>long数据</returns>
        private static long IPToLong(string ip)
        {
            string[] ipArr = ip.Split(new[] { '.' });
            return int.Parse(ipArr[0]) * (long)16777216 + int.Parse(ipArr[1]) * 65536 + int.Parse(ipArr[2]) * 256 + int.Parse(ipArr[3]);
        }


        #endregion
    }
}
#endif