using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace NCompose
{
    public static class Composable
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();
        private static readonly Type[] interfaces = new Type[] { typeof(IComposable) };

        public static T Create<T>(Action<IComposable> callback = null)
        {
            return Create<T>(CompositionBehavior.Default, callback);
        }

        public static T Create<T>(CompositionBehavior behavior)
        {
            return Create<T>(behavior, null);
        }

        public static T Create<T>(CompositionBehavior behavior, Action<IComposable> callback = null)
        {
            var type = typeof(T);
            if (!type.IsInterface)
            {
                throw new ArgumentException();
            }

            var interceptor = new Interceptor(behavior);
            var composable = generator.CreateInterfaceProxyWithoutTarget(type, interfaces, interceptor);

            if (callback != null)
            {
                callback(composable as IComposable);
            }

            return (T)composable;
        }
    }

    public class Interceptor : IInterceptor, IComposable
    {
        private readonly CompositionBehavior behavior;
        private readonly IList<object> parts = new List<object>();

        public Interceptor(CompositionBehavior behavior)
        {
            this.behavior = behavior;
        }

        void IInterceptor.Intercept(IInvocation invocation)
        {
            MethodInfo method;
            object target;

            if (TryGetInvokeInfo(invocation, out method, out target))
            {
                invocation.ReturnValue = method.Invoke(target, invocation.Arguments);
            }
            else if (behavior == CompositionBehavior.Loose)
            {
                var type = invocation.Method.ReturnType;
                invocation.ReturnValue = type.IsValueType ?
                    Activator.CreateInstance(type) :
                    null;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        void IComposable.AddPart(object part)
        {
            parts.Add(part);
        }

        private bool TryGetInvokeInfo(IInvocation invocation, out MethodInfo method, out object target)
        {
            if (invocation.Method.DeclaringType == typeof(IComposable))
            {
                method = invocation.Method;
                target = this;

                return true;
            }

            foreach (var part in parts)
            {
                var type = part.GetType();
                method = type.GetMethod(invocation.Method.Name);

                if (method != null)
                {
                    target = part;

                    return true;
                }
            }

            method = null;
            target = null;

            return false;
        }
    }
}
