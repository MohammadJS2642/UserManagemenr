using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

public class UserRepository(UserManagementDbContext context) : IUserRepository
{
    public async Task AddAsync(User user) => await context.Users.AddAsync(user);

    public async Task<IEnumerable<User>> GetAllAsync() => await context.Users.ToListAsync();

    public async Task<User?> GetByIdAsync(int id) => await context.Users.SingleAsync(c => c.Id == id);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
