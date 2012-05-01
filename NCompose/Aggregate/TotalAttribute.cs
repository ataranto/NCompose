using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace NCompose.Aggregate
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
    public class TotalAttribute : Attribute
    {
        public object GetResult(IInvocation invocation, ICollection<object> parts)
        {
            object result = null;
            var add = GetAdd(invocation.Method.ReturnType);

            foreach (var part in parts)
            {
                var type = part.GetType();
                var method = type.GetMethod(invocation.Method.Name);

                if (method != null &&
                    method.ReturnType == invocation.Method.ReturnType)
                {
                    var value = method.Invoke(part, invocation.Arguments);
                    result = result == null ?
                        value :
                        add(result, value);
                }
            }

            return result;
        }

        private static Func<object, object, object> GetAdd(Type type)
        {
            if (type == typeof(int))
            {
                return (a, b) =>  { return (int)a + (int)b; };
            }
            else
            {
                return (a, b) => { return null; };
            }
        }
    }
}
