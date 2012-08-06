using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace NCompose
{
    public class Composable : IInterceptor, IComposable
    {
        private readonly CompositionBehavior behavior;
        private HashSet<object> parts = new HashSet<object>();
        private IDictionary<string, Aggregate> aggregates = new Dictionary<string, Aggregate>();

        public Composable(CompositionBehavior behavior)
        {
            this.behavior = behavior;
        }

        void IInterceptor.Intercept(IInvocation invocation)
        {
            Aggregate aggregate;
            MethodInfo method;
            object target;

            if (aggregates.TryGetValue(invocation.Method.Name, out aggregate))
            {
                method = invocation.Method.ReflectedType.GetMethod(invocation.Method.Name);

                foreach (var part in parts)
                {
                    var value = method.Invoke(part, invocation.Arguments);
                    invocation.ReturnValue = invocation.ReturnValue == null ?
                        value :
                        aggregate.Func(invocation.ReturnValue, value);
                }
            }
            else if (TryGetInvokeInfo(invocation, out method, out target))
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

        void IComposable.AddAggregate<T>(string name, Func<T, T, T> func)
        {
            var aggregate = new Aggregate<T>(func);
            aggregates.Add(name, aggregate);
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
