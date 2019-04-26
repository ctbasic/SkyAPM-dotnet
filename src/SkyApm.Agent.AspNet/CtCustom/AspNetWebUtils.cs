using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Xml.Linq;
using SkyApm.Tracing.Segments;

namespace SkyApm.Agent.AspNet.CtCustom
{
    internal static class AspNetWebUtils
    {
        /// <summary>
        /// 头信息分隔符
        /// </summary>
        private const char HEADER_SEPARATOR = '|';

        public const string SEPARATOR = ":";

        /// <summary>
        /// 头信息键值对分隔符
        /// </summary>
        private const char HEADER_KEY_VALUE_SEPARATOR = '=';

        /// <summary>
        /// RPC名称
        /// </summary>
        public const string SOAP_RPC_NAME = "soap";


        /// <summary>
        /// 上下文中的trace
        /// </summary>
        private const string HTTPCONTEXT_SKYAMP_TRACE = "httpcontext_skyapm_trace";


        public static SegmentContext SegmentContext
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }
                return HttpContext.Current.Items[HTTPCONTEXT_SKYAMP_TRACE] as SegmentContext;
            }
            set
            {
                HttpContext.Current.Items[HTTPCONTEXT_SKYAMP_TRACE] = value;
            }
        }
    }
}