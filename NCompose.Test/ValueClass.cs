namespace NCompose.Test
{
    internal class ValueClass<T> : IValueInterface<T>
    {
        private readonly T value;

        public ValueClass(T value)
        {
            this.value = value;
        }

        public T ValueProperty
        {
            get { return value; }
        }

        public T ValueMethod()
        {
            return value;
        }
    }
}
