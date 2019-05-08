using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thrift.Server
{
    public class ThriftLog
    {
        /// <summary>
        /// info事件
        /// </summary>
        public static Action<string> _eventInfo = null;

        /// <summary>
        /// error事件
        /// </summary>
        public static Action<string> _eventError = null;

        static ThriftLog()
        {
        }

        public static void Info(string msg)
        {
#if DEBUG
            Console.WriteLine(msg);
#endif

            if (_eventInfo != null)
                _eventInfo("ThriftServer:" + msg);
        }
        public static void Error(string msg)
        {
#if DEBUG
            Console.WriteLine(msg);
#endif

            if (_eventError != null)
                _eventError("ThriftServer:" + msg);
        }
    }
}
