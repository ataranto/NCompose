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

    public interface ICompleteInterface
    {
        int Property { get; set; }
        void Out(out int x);
        void Ref(ref int x);
        T Generic<T>();
    }

    internal class CompleteClass : ICompleteInterface
    {
        public int Property
        {
            get
            {
                return 1;
            }

            set
            {
                
            }
        }

        public void Out(out int x)
        {
            x = 1;
        }

        public void Ref(ref int x)
        {
            x = 1;
        }

        public T Generic<T>()
        {
            return default(T);
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

        ////

        [Fact]
        private void CallsGetProperty()
        {
            var test = GetCompleteComposable();
            Assert.Equal(1, test.Property);
        }

        [Fact]
        private void CallsSetProperty()
        {
            var test = GetCompleteComposable();
            test.Property = 1;
        }

        [Fact]
        private void CallsOutMethod()
        {
            var test = GetCompleteComposable();

            int x;
            test.Out(out x);
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsRefMethod()
        {
            var test = GetCompleteComposable();

            int x = 0;
            test.Ref(ref x);
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsGenericMethod()
        {
            var test = GetCompleteComposable();
            Assert.Equal(default(int), test.Generic<int>());
        }

        private static ICompleteInterface GetCompleteComposable()
        {
            return Composable.Create<ICompleteInterface>(composable =>
            {
                composable.AddPart(new CompleteClass());
            });
        }
    }
}
