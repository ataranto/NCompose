using System;

namespace NCompose
{
    internal abstract class Aggregate
    {
        public object Func(object a, object b)
        {
            return InternalFunc(a, b);
        }

        protected abstract object InternalFunc(object a, object b);
    }

    internal class Aggregate<T> : Aggregate
    {
        private readonly Func<T, T, T> func;

        public Aggregate(Func<T, T, T> func)
        {
            this.func = func;
        }

        protected override object InternalFunc(object a, object b)
        {
            return this.func((T)a, (T)b);
        }
    }
}
