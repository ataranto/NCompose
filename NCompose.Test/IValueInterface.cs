namespace NCompose.Test
{
    internal interface IValueInterface<T>
    {
        T ValueProperty { get; }
        T ValueMethod();
    }
}
