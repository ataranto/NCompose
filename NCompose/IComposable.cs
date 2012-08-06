using System.Collections.Generic;
using System;
namespace NCompose
{
    public interface IComposable
    {
        IList<object> Parts { get; }
        void AddPart(object part);
        void AddAggregate<T>(string name, Func<T, T, T> func);
    }
}