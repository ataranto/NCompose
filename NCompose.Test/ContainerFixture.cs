using Xunit;

namespace NCompose.Test
{
    public interface ITestInterface
    {
        bool Method1();
        bool Method2();
    }

    internal class Class1
    {
        public bool Method1()
        {
            return true;
        }
    }

    internal class Class2
    {
        public bool Method2()
        {
            return true;
        }
    }

    public class ContainerFixture
    {
        [Fact]
        public void TestBasicSyntax()
        {
            var test = ComposableFactory.CreateComposable<ITestInterface>(composable =>
            {
                composable.Add(new Class1());
                composable.Add(new Class2());
            });

            Assert.True(test.Method1());
            Assert.True(test.Method2());
        }

        ////

        [Fact]
        public void ComposableImplementsInterface()
        {
            var composable = ComposableFactory.CreateComposable<ITestInterface>();
            Assert.IsAssignableFrom<IComposable>(composable);
        }

        [Fact]
        public void AddsElement()
        {
            ComposableFactory.CreateComposable<ITestInterface>(composable =>
            {
                composable.Add(new Class1());
            });
        }
    }
}
