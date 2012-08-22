using NCompose.Interceptors;
using Xunit;
using System;

namespace NCompose.Test.Interceptors
{
    public class ReduceFixture
    {
        private static int Count = 5;

        [Fact]
        private void ReducesSum()
        {
            var interceptor = new Reduce<int>("ValueMethod", SumFunc);
            var test = GetComposable(interceptor);

            Assert.Equal(15, test.ValueMethod());
        }

        [Fact]
        private void ReducesMin()
        {
            var interceptor = new Reduce<int>("ValueMethod", MinFunc);
            var test = GetComposable(interceptor);

            Assert.Equal(1, test.ValueMethod());
        }

        [Fact]
        private void ReducesMax()
        {
            var interceptor = new Reduce<int>("ValueMethod", MaxFunc);
            var test = GetComposable(interceptor);

            Assert.Equal(5, test.ValueMethod());
        }

        private static IValueInterface<int> GetComposable(IInterceptor interceptor)
        {
            var test = Composable.Create<IValueInterface<int>>(composable =>
            {
                for (var x = 0; x < Count; x++)
                {
                    var part = new ValueClass<int>(x + 1);
                    composable.AddPart(part);
                }

                composable.AddInterceptor(interceptor);
            });
            return test;
        }

        private static int SumFunc(int a, int b)
        {
            return a + b;
        }

        private static int MinFunc(int a, int b)
        {
            return Math.Min(a, b);
        }

        private static int MaxFunc(int a, int b)
        {
            return Math.Max(a, b);
        }
    }
}
