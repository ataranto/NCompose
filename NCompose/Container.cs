using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace NCompose
{
    public static class ComposableFactory
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();
        private static readonly Type[] interfaces = new Type[] { typeof(IComposable) };
        
        public static T CreateComposable<T>(Action<IComposable> callback = null) where T : class
        {
            var type = typeof(T);
            if (!type.IsInterface)
            {
                throw new ArgumentException();
            }

            var composable = generator.CreateInterfaceProxyWithoutTarget(type, interfaces, new Interceptor());
            if (callback != null)
            {
                callback(composable as IComposable);
            }

            return composable as T;
        }
    }

    public class Interceptor : IInterceptor, IComposable
    {
        private readonly IList<object> elements = new List<object>();

        void IInterceptor.Intercept(IInvocation invocation)
        {
            if (invocation.Method.DeclaringType == typeof(IComposable))
            {
                invocation.ReturnValue =
                    invocation.Method.Invoke(this, invocation.Arguments);
            }
            else
            {
                foreach (var element in elements)
                {
                    var type = element.GetType();
                    var method = type.GetMethod(invocation.Method.Name);

                    if (method != null)
                    {
                        invocation.ReturnValue =
                            method.Invoke(element, invocation.Arguments);
                    }
                }
            }
        }

        void IComposable.Add(object element)
        {
            elements.Add(element);
        }
    }
}
