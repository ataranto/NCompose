using System;
using Xunit;

namespace NCompose.Test
{
    public interface ICompleteInterface
    {
        int Property { get; set; }
        void Out(out int x);
        void Ref(ref int x);
        int ReturnAndOut(out int x);
        int ReturnAndRef(ref int x);
        T Generic<T>();
    }

    internal class CompleteClass : ICompleteInterface
    {
        public int Property
        {
            get
            {
                return 1;
            }

            set
            {

            }
        }

        public void Out(out int x)
        {
            x = 1;
        }

        public void Ref(ref int x)
        {
            x = 1;
        }

        public int ReturnAndOut(out int x)
        {
            x = 1;
            return 1;
        }

        public int ReturnAndRef(ref int x)
        {
            x = 1;
            return 1;
        }

        public T Generic<T>()
        {
            return default(T);
        }
    }

    public interface IValueInterface
    {
        int ValueProperty { get; }
        int ValueMethod();
    }

    public class ValueClass : IValueInterface
    {
        private readonly int value;

        public ValueClass(int value)
        {
            this.value = value;
        }

        public int ValueProperty
        {
            get { return value; }
        }

        public int ValueMethod()
        {
            return value;
        }
    }

    public class ComposableFixture
    {
        private static int Count = 3;

        [Fact]
        private void CallsGetProperty()
        {
            var test = GetCompleteComposable();
            Assert.Equal(1, test.Property);
        }

        [Fact]
        private void CallsSetProperty()
        {
            var test = GetCompleteComposable();
            test.Property = 1;
        }

        [Fact]
        private void CallsOutMethod()
        {
            var test = GetCompleteComposable();

            int x;
            test.Out(out x);
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsRefMethod()
        {
            var test = GetCompleteComposable();

            int x = 0;
            test.Ref(ref x);
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsReturnAndOutMethod()
        {
            var test = GetCompleteComposable();

            int x;
            Assert.Equal(1, test.ReturnAndOut(out x));
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsReturnAndRefMethod()
        {
            var test = GetCompleteComposable();

            int x = 0;
            Assert.Equal(1, test.ReturnAndRef(ref x));
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsGenericMethod()
        {
            var test = GetCompleteComposable();
            Assert.Equal(default(int), test.Generic<int>());
        }

        [Fact]
        private void CallsAggregateProperty()
        {
            var test = GetValueComposable();
            Assert.Equal(6, test.ValueProperty);
        }

        [Fact]
        private void CallsAggregateMethod()
        {
            var test = GetValueComposable();
            Assert.Equal(6, test.ValueMethod());
        }

        private static ICompleteInterface GetCompleteComposable()
        {
            return ComposableFactory.Create<ICompleteInterface>(composable =>
            {
                composable.AddPart(new CompleteClass());
            });
        }

        private static IValueInterface GetValueComposable()
        {
            return ComposableFactory.Create<IValueInterface>(composable =>
            {
                for (var x = 0; x < Count; x++)
                {
                    var part = new ValueClass(x + 1);
                    composable.AddPart(part);
                }

                Func<int, int, int> func = (a, b) => { return a + b; };

                composable.AddAggregate<int>("ValueProperty", func);
                composable.AddAggregate<int>("ValueMethod", func);
            });
        }
    }
}
