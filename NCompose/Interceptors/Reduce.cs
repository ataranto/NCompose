using System.Collections.Generic;
using Castle.DynamicProxy;

namespace NCompose.Interceptors
{
    public delegate T ReduceFunc<T>(T a, T b);

    public class Reduce<T> : IInterceptor
    {
        private readonly string name;
        private readonly ReduceFunc<T> func;

        public Reduce(string name, ReduceFunc<T> func)
        {
            this.name = name;
            this.func = func;
        }

        public bool TryIntercept(IInvocation invocation, IEnumerable<object> parts)
        {
            if (invocation.Method.Name != name)
            {
                return false;
            }

            var queue = new Queue<object>(2);
            foreach (var part in parts)
            {
                var type = part.GetType();
                var method = type.GetMethod(name);

                if (method == null)
                {
                    continue;
                }

                queue.Enqueue(method.Invoke(part, invocation.Arguments));

                if (queue.Count == 2)
                {
                    invocation.ReturnValue = func((T)queue.Dequeue(), (T)queue.Dequeue());
                    queue.Enqueue(invocation.ReturnValue);
                }
            }

            return true;
        }
    }
}