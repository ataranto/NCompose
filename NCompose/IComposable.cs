using System.Collections.Generic;
using System;
namespace NCompose
{
    public interface IComposable
    {
        bool AddPart(object part);
        bool AddInterceptor(IInterceptor interceptor);
    }
}