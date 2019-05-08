#if NETCORE

using System;
using System.Diagnostics;
using System.Reflection;
using System.ReflectionMy;

namespace Thrift.Server
{
    public class InvokeProxy<T> : DispatchProxyMy
    {
        private Type type = null;
        public InvokeProxy()
        {
            type = typeof(T);
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {

            var url = targetMethod.Name;

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var instance = Activator.CreateInstance(type);
                object returnValue = targetMethod.Invoke(instance, args);

                watch.Stop();
                var time = watch.ElapsedMilliseconds;

                if (Server._funcTime != null)
                    Server._funcTime(url, args, time);

                return returnValue;
            }
            catch (Exception ex)
            {
                if (Server._funcError != null)
                    Server._funcError(url, args, ex);
                throw ex;
            }
        }
    }


    public static class TransparentProxy
    {
        public static object Create(Type InvokeProxy, Type T)
        {
            return DispatchProxyMy.Create(InvokeProxy,T);
        }
    }
}

#else


using System;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;    //RealProxy

namespace Thrift.Server
{

    public class InvokeProxy : RealProxy
    {
        private object _target;

        public InvokeProxy(object target) : base(target.GetType())
        {
            this._target = target;
        }
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMessage = (IMethodCallMessage)msg;
            var url = callMessage.MethodName;
            var args = callMessage.Args;

            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                object returnValue = callMessage.MethodBase.Invoke(this._target, callMessage.Args);

                watch.Stop();
                var time = watch.ElapsedMilliseconds;

                if (Server._funcTime != null)
                    Server._funcTime(url, args, time);

                return new ReturnMessage(returnValue, new object[0], 0, null, callMessage);
            }
            catch (Exception ex)
            {
                if (Server._funcError != null)
                    Server._funcError(url, args,ex);
                throw ex;
            }
        }
    }

    public static class TransparentProxy
    {
        public static object Create(Type t)
        {
            var instance = Activator.CreateInstance(t);

            InvokeProxy realProxy = new InvokeProxy(instance);
            var transparentProxy = realProxy.GetTransparentProxy();
            return transparentProxy;
        }
    }
}
#endif