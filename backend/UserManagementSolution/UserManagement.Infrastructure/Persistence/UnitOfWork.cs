using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Persistence;

public class UnitOfWork(UserManagementDbContext _context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
}
