namespace NCompose.Test
{
    public interface IValueInterface<T>
    {
        T ValueProperty { get; }
        T ValueMethod();
    }
}
