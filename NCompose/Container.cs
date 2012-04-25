using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
namespace NCompose
{
    public interface IComposable
    {
        void Add(object element);
    }

    public interface IFoo
    {
        bool Bar();
        bool Baz();
    }

    public class ComposableFactory
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();
        private static readonly Type[] interfaces = new Type[] { typeof(IComposable) };
        
        public static T CreateComposable<T>(Action<IComposable> callback = null)
        {
            var type = typeof(T);
            if (!type.IsInterface)
            {
                throw new ArgumentException();
            }

            var options = new ProxyGenerationOptions
            {
                BaseTypeForInterfaceProxy = typeof(Interceptor),
                Selector = new Selector(),
            };

            var composable = (T)generator.CreateInterfaceProxyWithoutTarget(type, interfaces, options, new Interceptor());
            if (callback != null)
            {
                callback(composable as IComposable);
            }

            return composable;
        }
    }

    public class Selector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            Console.WriteLine("SElect: {0}, {1}", type, method);
            return interceptors;
        }
    }

    public class Interceptor : IInterceptor, IComposable, IFoo
    {
        IList<object> elements = new List<object>();

        public void Intercept(IInvocation invocation)
        {
            //if (invocation.Method.Name == "Bar")
                Console.WriteLine("method: {0}", invocation.Method.Name);

            if (invocation.InvocationTarget == null
                && invocation.Method.Name == "Add")
            {
                Console.WriteLine("Interceptor Adding");
                elements.Add(invocation.Arguments);
            }
            else if (invocation.InvocationTarget != null)
            {
                Console.WriteLine("got target: {0}", invocation.InvocationTarget);
            }


            //var target = invocation.InvocationTarget ?? invocation.Proxy;
            //var composable = target as IComposable;

            /*
            int x = 0;
            foreach (var element in composable.Elements)
            {
                Console.WriteLine("[{0}] {1}", x++, element.GetType());
            }
            */

            //Console.WriteLine(composable.GetType());
            //Console.WriteLine("+++++++ {0}", invocation.InvocationTarget);
            //Console.WriteLine(invocation);
            //throw new NotImplementedException();

            invocation.ReturnValue = false;
        }

        public void Add(object element)
        {
        }

        public bool Bar()
        {
            return false;
        }

        public bool Baz()
        {
            return false;
        }
    }
}
