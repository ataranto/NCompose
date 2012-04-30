using NCompose.Aggregate;
using Xunit;

namespace NCompose.Test.Aggregate
{
    public interface ICount
    {
        [Total]
        int Count { get; }
    }

    internal class Total
    {
        public int Count
        {
            get { return 1; }
        }
    }

    public class TotalFixture
    {
        [Fact]
        private void TotalReturnsSum()
        {
            var count = 10;
            var test = ComposableFactory.Create<ICount>(composable =>
            {
                for (var x = 0; x < count; x++)
                {
                    composable.AddPart(new Total());
                }
            });

            Assert.Equal(count, test.Count);
        }
    }
}
