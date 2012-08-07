using System.Collections.Generic;
using Castle.DynamicProxy;

namespace NCompose
{
    public interface IInterceptor
    {
        bool TryIntercept(IInvocation invocation, IEnumerable<object> parts);
    }
}