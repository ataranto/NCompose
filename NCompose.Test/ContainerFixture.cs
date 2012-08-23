using Castle.DynamicProxy;
using Moq;
using Xunit;
using System.Reflection;
using System.Globalization;

namespace NCompose.Test
{
    public interface ICompleteInterface
    {
        int Property { get; set; }
        void Out(out int x);
        void Ref(ref int x);
        int ReturnAndOut(out int x);
        int ReturnAndRef(ref int x);
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

        public int ReturnAndOut(out int x)
        {
            x = 1;
            return 1;
        }

        public int ReturnAndRef(ref int x)
        {
            x = 1;
            return 1;
        }

        public T Generic<T>()
        {
            return default(T);
        }
    }

    public class ContainerFixture : IUseFixture<MoqFixture>
    {
        private Mock<IInvocation> mock_invocation;

        public void SetFixture(MoqFixture moq_fixture)
        {
            mock_invocation = moq_fixture.Create<IInvocation>();
        }

        [Fact]
        private void Foo()
        {
            var container = new Container(CompositionBehavior.Default);
            var interceptor = container as Castle.DynamicProxy.IInterceptor;

            var arguments = new object[] { new object() };
            var return_value = new object();

            var mock_method = Mock.Get<MethodInfo>(mock_invocation.Object.Method);
            
            mock_method.
                Setup(m => m.DeclaringType).
                Returns(typeof(IComposable));
            mock_invocation.
                Setup(m => m.Arguments).
                Returns(arguments);
            mock_method.
                Setup(m => m.Invoke(container, It.IsAny<BindingFlags>(), It.IsAny<Binder>(), arguments, It.IsAny<CultureInfo>())).
                Returns(return_value);
            mock_invocation.
                SetupSet(m => m.ReturnValue = return_value);

            interceptor.Intercept(mock_invocation.Object);
        }
    }

    public class OldContainerFixture
    {
        [Fact]
        private void CallsGetProperty()
        {
            var test = GetComposable();
            Assert.Equal(1, test.Property);
        }

        [Fact]
        private void CallsSetProperty()
        {
            var test = GetComposable();
            test.Property = 1;
        }

        [Fact]
        private void CallsOutMethod()
        {
            var test = GetComposable();

            int x;
            test.Out(out x);
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsRefMethod()
        {
            var test = GetComposable();

            int x = 0;
            test.Ref(ref x);
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsReturnAndOutMethod()
        {
            var test = GetComposable();

            int x;
            Assert.Equal(1, test.ReturnAndOut(out x));
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsReturnAndRefMethod()
        {
            var test = GetComposable();

            int x = 0;
            Assert.Equal(1, test.ReturnAndRef(ref x));
            Assert.Equal(1, x);
        }

        [Fact]
        private void CallsGenericMethod()
        {
            var test = GetComposable();
            Assert.Equal(default(int), test.Generic<int>());
        }

        private static ICompleteInterface GetComposable()
        {
            return Composable.Create<ICompleteInterface>(composable =>
            {
                composable.AddPart(new CompleteClass());
            });
        }
    }
}
