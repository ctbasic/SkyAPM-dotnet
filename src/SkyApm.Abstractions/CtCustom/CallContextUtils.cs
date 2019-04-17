#if NET_FX
using System.Runtime.Remoting.Messaging;
#endif


#if NETSTANDARD
using System.Collections.Concurrent;
using System.Threading;

#endif

namespace SkyApm.Abstractions.Common
{
    public static class CallContextUtils
    {
        /// <summary>
        /// key前缀
        /// </summary>
        private const string KEY_PREFIX = "SkyApm_";

#if NETSTANDARD
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public static void SetData(string name, object data)
        {
            string key = KEY_PREFIX + name;
            state.GetOrAdd(key, o => new AsyncLocal<object>()).Value = data;
        }


        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetData(string name)
        {
            string key = KEY_PREFIX + name;
            return state.TryGetValue(key, out AsyncLocal<object> data) ? data.Value : null;
        }

#endif
#if NET_FX

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public static void SetData(string name, object data)
        {
            string key = KEY_PREFIX + name;
            CallContext.LogicalSetData(key, data);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetData(string name)
        {
            string key = KEY_PREFIX + name;
            return CallContext.LogicalGetData(key);
        }
#endif
    }
}
