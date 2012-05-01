using NCompose.Aggregate;
using Xunit;

namespace NCompose.Test.Aggregate
{
    public interface IMax
    {
        [Max]
        int MaxProperty { get; }

        [Max]
        int MaxMathod();
    }

    internal class Max : IMax
    {
        private readonly int max;

        public Max(int max)
        {
            this.max = max;
        }

        public int MaxProperty
        {
            get { return max; }
        }

        public int MaxMathod()
        {
            return max;
        }
    }

    public class MaxAttributeFixture
    {
        private static int Count = 3;

        [Fact]
        private void PropertyReturnsMax()
        {
            var test = GetComposable();
            Assert.Equal(Count, test.MaxProperty);
        }

        [Fact]
        private void MethodReturnsMax()
        {
            var test = GetComposable();
            Assert.Equal(Count, test.MaxMathod());
        }

        private static IMax GetComposable()
        {
            return ComposableFactory.Create<IMax>(composable =>
            {
                for (var x = 0; x < Count; x++)
                {
                    composable.AddPart(new Max(x + 1));
                }
            });
        }
    }
}
