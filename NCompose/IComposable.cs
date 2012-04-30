using System.Collections.Generic;
namespace NCompose
{
    public interface IComposable
    {
        IList<object> Parts { get; }
        void AddPart(object part);
    }
}