using Xunit;
using System;

namespace NCompose.Test
{
    public interface ITest
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

    public class ComposableFixture
    {
        [Fact]
        public void TestBasicCreateSyntax()
        {
            var test = Composable.Create<ITest>(composable =>
            {
                composable.AddPart(new Class1());
                composable.AddPart(new Class2());
            });

            Assert.True(test.Method1());
            Assert.True(test.Method2());
        }

        ////

        [Fact]
        public void ComposableImplementsInterface()
        {
            var composable = Composable.Create<ITest>();
            Assert.IsAssignableFrom<IComposable>(composable);
        }

        [Fact]
        public void AddsPart()
        {
            Composable.Create<ITest>(composable =>
            {
                composable.AddPart(new Class1());
            });
        }

        [Fact]
        public void LooseBehaviorReturnsDefault()
        {
            var test = Composable.Create<ITest>(CompositionBehavior.Loose);
            Assert.Equal(default(bool), test.Method1());
            Assert.Equal(default(bool), test.Method2());
        }

        [Fact]
        public void StrictBehaviorThrowsException()
        {
            var test = Composable.Create<ITest>(CompositionBehavior.Strict);
            Assert.Throws<InvalidOperationException>(() => test.Method1());
        }
    }
}
