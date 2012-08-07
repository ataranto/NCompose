using NCompose.Interceptors;
using Xunit;

namespace NCompose.Test.Interceptors
{
    public class ReduceFixture
    {
        private int Count = 5;
        private int Sum = 15;

        [Fact]
        private void Reduce()
        {
            var test = ComposableFactory.Create<IValueInterface<int>>(composable =>
            {
                for (var x = 0; x < Count; x++)
                {
                    var part = new ValueClass<int>(x + 1);
                    composable.AddPart(part);
                }

                var interceptor = new Reduce<int>("ValueMethod", SumFunc);
                composable.AddInterceptor(interceptor);
            });

            Assert.Equal(Sum, test.ValueMethod());
        }

        private static int SumFunc(int a, int b)
        {
            return a + b;
        }
    }
}
