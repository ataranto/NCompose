using NCompose.Aggregate;
using Xunit;

namespace NCompose.Test.Aggregate
{
    public interface ICount
    {
        [Total]
        int CountProperty { get; }

        [Total]
        int CountMethod();
    }

    internal class Total : ICount
    {
        public int CountProperty
        {
            get { return 1; }
        }

        public int CountMethod()
        {
            return 1;
        }
    }

    public class TotalAttributeFixture
    {
        private static int Count = 10;

        [Fact]
        private void PropertyReturnsTotal()
        {
            var test = GetComposable();
            Assert.Equal(Count, test.CountProperty);
        }

        [Fact]
        private void MethodReturnsTotal()
        {
            var test = GetComposable();
            Assert.Equal(Count, test.CountMethod());
        }

        private static ICount GetComposable()
        {
            return ComposableFactory.Create<ICount>(composable =>
            {
                for (var x = 0; x < Count; x++)
                {
                    composable.AddPart(new Total());
                }
            });
        }
    }
}
