using System.Collections.Generic;
using Castle.DynamicProxy;

namespace NCompose
{
    interface IInterceptor
    {
        void Intercept(IInvocation invocation, IEnumerable<object> parts);
    }
}
