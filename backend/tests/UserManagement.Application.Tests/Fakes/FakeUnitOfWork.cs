using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Tests.Fakes;

internal class FakeUnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(1);
    }
}
