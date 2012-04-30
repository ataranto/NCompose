using System;
using Castle.DynamicProxy;

namespace NCompose
{
    public static class ComposableFactory
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

            var interceptor = new Composable(behavior);
            var composable = generator.CreateInterfaceProxyWithoutTarget(type, interfaces, interceptor);

            if (callback != null)
            {
                callback(composable as IComposable);
            }

            return (T)composable;
        }
    }
}
