using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace NCompose
{
    internal class Container : Castle.DynamicProxy.IInterceptor, IComposable
    {
        private readonly CompositionBehavior behavior;
        private HashSet<object> parts = new HashSet<object>();
        private HashSet<IInterceptor> interceptors = new HashSet<IInterceptor>();

        public Container(CompositionBehavior behavior)
        {
            this.behavior = behavior;
        }

        void Castle.DynamicProxy.IInterceptor.Intercept(IInvocation invocation)
        {
            // invoke any IComposable methods on this instance
            if (invocation.Method.DeclaringType == typeof(IComposable))
            {
                invocation.ReturnValue = invocation.Method.Invoke(this, invocation.Arguments);
                return;
            }

            foreach (var interceptor in interceptors)
            {
                if (interceptor.TryIntercept(invocation, parts))
                {
                    return;
                }
            }

            foreach (var part in parts)
            {
                var type = part.GetType();
                var method = type.GetMethod(invocation.Method.Name);

                if (method != null)
                {
                    invocation.ReturnValue = method.Invoke(part, invocation.Arguments);
                    return;
                }
            }

            if (behavior == CompositionBehavior.Loose)
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

        bool IComposable.AddPart(object part)
        {
            return parts.Add(part);
        }

        bool IComposable.AddInterceptor(IInterceptor interceptor)
        {
            return interceptors.Add(interceptor);
        }
    }
}
