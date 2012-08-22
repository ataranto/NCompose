using System;
using Xunit;

namespace NCompose.Test
{
    public interface ISimpleInterface
    {
        bool Method1();
        bool Method2();
    }

    internal class SimpleClass1
    {
        public bool Method1()
        {
            return true;
        }
    }

    internal class SimpleClass2
    {
        public bool Method2()
        {
            return true;
        }
    }

    public class ComposableFixture
    {
        [Fact]
        private void TestBasicCreateSyntax()
        {
            var test = Composable.Create<ISimpleInterface>(composable =>
            {
                composable.AddPart(new SimpleClass1());
                composable.AddPart(new SimpleClass2());
            });

            Assert.True(test.Method1());
            Assert.True(test.Method2());
        }

        ////

        [Fact]
        private void ComposableImplementsInterface()
        {
            var composable = Composable.Create<ISimpleInterface>();
            Assert.IsAssignableFrom<IComposable>(composable);
        }

        [Fact]
        private void AddsPart()
        {
            Composable.Create<ISimpleInterface>(composable =>
            {
                composable.AddPart(new SimpleClass1());
            });
        }

        [Fact]
        private void DuplicatePartsCannotBeAdded()
        {
            var composable = Composable.Create<ISimpleInterface>() as IComposable;
            var part = new SimpleClass1();

            Assert.True(composable.AddPart(part));
            Assert.False(composable.AddPart(part));
        }

        [Fact]
        private void LooseBehaviorReturnsDefault()
        {
            var test = Composable.Create<ISimpleInterface>(CompositionBehavior.Loose);
            Assert.Equal(default(bool), test.Method1());
            Assert.Equal(default(bool), test.Method2());
        }

        [Fact]
        private void StrictBehaviorThrowsException()
        {
            var test = Composable.Create<ISimpleInterface>(CompositionBehavior.Strict);
            Assert.Throws<InvalidOperationException>(() => test.Method1());
        }

        [Fact]
        private void DefaultBehaviorIsStrict()
        {
            var test = Composable.Create<ISimpleInterface>();
            Assert.Throws<InvalidOperationException>(() => test.Method1());
        }
    }
}
