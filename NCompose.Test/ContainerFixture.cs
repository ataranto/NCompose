using Xunit;
namespace NCompose.Test
{
    internal class BarClass
    {
        bool Bar()
        {
            return true;
        }
    }

    internal class BazClass
    {
        bool Baz()
        {
            return true;
        }
    }

    public class ContainerFixture
    {
        [Fact]
        public void TestBasicSyntax()
        {
            var foo = ComposableFactory.CreateComposable<IFoo>(composable =>
            {
                composable.Add(new BarClass());
                composable.Add(new BazClass());
            });

            Assert.True(foo.Bar());
            Assert.True(foo.Baz());
        }

        ////

        [Fact]
        public void ComposableImplementsInterface()
        {
            var composable = ComposableFactory.CreateComposable<IFoo>();

            Assert.IsAssignableFrom<IComposable>(composable);
        }

        [Fact]
        public void AddsElement()
        {
            ComposableFactory.CreateComposable<IFoo>(composable =>
            {
                composable.Add(new BarClass());
            });
        }
    }
}
