using System;
using System.Collections.Generic;
using System.Linq;
using Thrift.Protocol;

namespace SkyApm.Agent.Thrift.ProtocolExt
{
    internal static class ProtocolUtils
    {
        /// <summary>
        /// 头信息不同键值对信息分隔符
        /// </summary>
        private const char HEADER_SEPARATOR = ';';

        /// <summary>
        /// 头信息键值对分隔符
        /// </summary>
        private const char HEADER_KEY_VALUE_SEPARATOR = '|';

        /// <summary>
        /// 头信息与rpc方法名分隔符
        /// </summary>
        private const char HEADER_RPCMETHOD_SEPARATOR = '*';

        /// <summary>
        /// RPC名称
        /// </summary>
        public const string RPC_NAME = "thrift.rpc";


        public static ThriftHeaders GetBinaryProtocolHeader(string sourceMsgName, out string targetMsgName)
        {
            try
            {
                ThriftHeaders thriftHeaders = new ThriftHeaders();
                targetMsgName = sourceMsgName;
                // 判断是否有分割标识
                if (!sourceMsgName.Contains(HEADER_RPCMETHOD_SEPARATOR))
                {
                    return thriftHeaders;
                }

                // 提取Header文本
                int index = targetMsgName.LastIndexOf(HEADER_RPCMETHOD_SEPARATOR.ToString(), StringComparison.Ordinal);
                string headersValue = targetMsgName.Substring(0, index);
                //将message.name还原，继续走thrift标准处理流程
                int len = headersValue.Length + HEADER_RPCMETHOD_SEPARATOR.ToString().Length;
                targetMsgName = targetMsgName.Substring(len);

                string[] headers = headersValue.Split(new[] { HEADER_SEPARATOR });
                foreach (var keyValueStr in headers)
                {
                    //string[] keyValue = keyValueStr.Split(HEADER_KEY_VALUE_SEPARATOR);
                    string[] keyValue = keyValueStr.Split(HEADER_KEY_VALUE_SEPARATOR);
                    string key = "";
                    string value = "";
                    if (keyValue.Length == 2)
                    {
                        key = keyValue[0];
                        value = keyValue[1];
                    }
                    if (!string.IsNullOrWhiteSpace(key) && !thriftHeaders.Contains(key))
                    {
                        thriftHeaders.Add(key, value);
                    }
                }

                return thriftHeaders;
            }
            catch (Exception e)
            {
                throw new TProtocolException(TProtocolException.INVALID_DATA, $"GetBinaryProtocolHeader error:{e}");
            }
        }


        /// <summary>
        /// 包装头信息
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string WrapBinaryProtocolHeader(ThriftHeaders headers)
        {
            if (headers == null || headers.ToList().Count <= 0)
            {
                return string.Empty;
            }
            string headerStr = string.Empty;
            foreach (var keyValue in headers.ToList())
            {
                //headerStr += $"{keyValue.Key}{HEADER_KEY_VALUE_SEPARATOR}{keyValue.Value}{HEADER_SEPARATOR}";

                headerStr += $"{keyValue.Key}{HEADER_KEY_VALUE_SEPARATOR }{keyValue.Value}{HEADER_SEPARATOR}";
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

            return header + HEADER_RPCMETHOD_SEPARATOR + sourceMsgName;
        }
    }
}