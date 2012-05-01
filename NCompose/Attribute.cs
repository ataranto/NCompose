using System.Collections.Generic;
using Castle.DynamicProxy;
namespace NCompose
{
    public abstract class Attribute : System.Attribute
    {
        public abstract object GetResult(IInvocation invocation, ICollection<object> parts);
    }
}
