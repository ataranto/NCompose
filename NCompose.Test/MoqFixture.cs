using System;
using Moq;

namespace NCompose.Test
{
    public class MoqFixture : IDisposable
    {
        private readonly MockRepository repository;

        public MoqFixture()
        {
            repository = new MockRepository(MockBehavior.Default) {
                DefaultValue = DefaultValue.Mock,
            };
        }

        public Mock<T> Create<T>(MockBehavior behavior = MockBehavior.Default)
            where T : class
        {
            return repository.Create<T>(behavior);
        }

        public void Dispose()
        {
            repository.VerifyAll();
        }
    }
}
