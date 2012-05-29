using System.Reflection;

namespace NCompose
{
    public interface IInvocation
    {
        object[] Arguments { get; }
        MethodInfo Method { get; }
    }
}
