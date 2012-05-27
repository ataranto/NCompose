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

    public class ComposableFactoryFixture
    {
        [Fact]
        private void TestBasicCreateSyntax()
        {
            var test = ComposableFactory.Create<ISimpleInterface>(composable =>
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
            var composable = ComposableFactory.Create<ISimpleInterface>();
            Assert.IsAssignableFrom<IComposable>(composable);
        }

        [Fact]
        private void AddsPart()
        {
            ComposableFactory.Create<ISimpleInterface>(composable =>
            {
                composable.AddPart(new SimpleClass1());
            });
        }

        [Fact]
        private void PartsIsInitiallyEmpty()
        {
            var composable = ComposableFactory.Create<ISimpleInterface>() as IComposable;
            var parts = composable.Parts;

            Assert.NotNull(parts);
            Assert.Equal(0, parts.Count);
        }

        [Fact]
        private void CanGetParts()
        {
            var composable = ComposableFactory.Create<ISimpleInterface>() as IComposable;
            var part = new SimpleClass1();
            composable.AddPart(part);

            Assert.Equal(1, composable.Parts.Count);
            Assert.Same(part, composable.Parts[0]);
        }

        [Fact]
        private void DuplicatePartsCannotBeAdded()
        {
            var composable = ComposableFactory.Create<ISimpleInterface>() as IComposable;
            var part = new SimpleClass1();
            composable.AddPart(part);
            composable.AddPart(part);

            Assert.Equal(1, composable.Parts.Count);
        }

        [Fact]
        private void LooseBehaviorReturnsDefault()
        {
            var test = ComposableFactory.Create<ISimpleInterface>(CompositionBehavior.Loose);
            Assert.Equal(default(bool), test.Method1());
            Assert.Equal(default(bool), test.Method2());
        }

        [Fact]
        private void StrictBehaviorThrowsException()
        {
            var test = ComposableFactory.Create<ISimpleInterface>(CompositionBehavior.Strict);
            Assert.Throws<InvalidOperationException>(() => test.Method1());
        }

        [Fact]
        private void DefaultBehaviorIsStrict()
        {
            var test = ComposableFactory.Create<ISimpleInterface>();
            Assert.Throws<InvalidOperationException>(() => test.Method1());
        }
    }
}
