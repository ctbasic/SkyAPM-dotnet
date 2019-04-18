using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Thrift.Protocol;

namespace Thrift.ZipKin.ProtocolExt
{
    internal static class ProtocolUtils
    {
        /// <summary>
        /// 头信息分隔符
        /// </summary>
        private const char HEADER_SEPARATOR = '|';

        /// <summary>
        /// 头信息键值对分隔符
        /// </summary>
        private const char HEADER_KEY_VALUE_SEPARATOR = '=';

        /// <summary>
        /// RPC名称
        /// </summary>
        public const string RPC_NAME = "thrift.rpc";

        public static Dictionary<string, string> GetBinaryProtocolHeader(string sourceMsgName, out string targetMsgName)
        {
            try
            {
                targetMsgName = sourceMsgName;
                // 判断是否有分割标识
                if (!sourceMsgName.Contains(TMultiplexedProtocol.SEPARATOR))
                {
                    return new Dictionary<string, string>();
                }

                // 提取Header文本
                int index = targetMsgName.LastIndexOf(TMultiplexedProtocol.SEPARATOR, StringComparison.Ordinal);
                string headersValue = targetMsgName.Substring(0, index);
                //将message.name还原，继续走thrift标准处理流程
                int len = headersValue.Length + TMultiplexedProtocol.SEPARATOR.Length;
                targetMsgName = targetMsgName.Substring(len);

                string[] headers = headersValue.Split(new[] { HEADER_SEPARATOR });
                Dictionary<string, string> headerDic = new Dictionary<string, string>();
                foreach (var keyValueStr in headers)
                {
                    string[] keyValue = keyValueStr.Split(HEADER_KEY_VALUE_SEPARATOR);
                    string key = "";
                    string value = "";
                    if (keyValue.Length == 2)
                    {
                        key = keyValue[0];
                        value = keyValue[1];
                    }
                    if (!string.IsNullOrWhiteSpace(key) && !headerDic.ContainsKey(key))
                    {
                        headerDic.Add(key, value);
                    }
                }

                return headerDic;
            }
            catch (Exception e)
            {
                throw new TProtocolException(TProtocolException.INVALID_DATA, $"GetBinaryProtocolHeader error:{e}");
            }
        }

        /// <summary>
        /// 包装头信息
        /// </summary>
        /// <param name="headerDic"></param>
        /// <returns></returns>
        public static string WrapBinaryProtocolHeader(Dictionary<string, string> headerDic)
        {
            if (headerDic == null || headerDic.Count <= 0)
            {
                return string.Empty;
            }
            string headerStr = string.Empty;
            foreach (var keyValue in headerDic)
            {
                headerStr += $"{keyValue.Key}{HEADER_KEY_VALUE_SEPARATOR}{keyValue.Value}{HEADER_SEPARATOR}";
            }
            return headerStr.TrimEnd(HEADER_SEPARATOR);
        }

        /// <summary>
        /// 包装消息名称
        /// </summary>
        /// <param name="sourceMsgName"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static string WrapMessageName(string sourceMsgName, string header = "")
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                return sourceMsgName;
            }

            return header + TMultiplexedProtocol.SEPARATOR + sourceMsgName;
        }
    }
}