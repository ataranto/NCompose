using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace NCompose
{
    public class Composable : Castle.DynamicProxy.IInterceptor, IComposable
    {
        private readonly CompositionBehavior behavior;
        private HashSet<object> parts = new HashSet<object>();

        public Composable(CompositionBehavior behavior)
        {
            this.behavior = behavior;
        }

        void Castle.DynamicProxy.IInterceptor.Intercept(IInvocation invocation)
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

        IList<object> IComposable.Parts
        {
            get
            {
                return new List<object>(parts).AsReadOnly();
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
