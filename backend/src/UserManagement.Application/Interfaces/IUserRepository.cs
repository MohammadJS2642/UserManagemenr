using System.Linq.Expressions;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>>? predicate = null);
    Task AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync();
    Task SaveChangesAsync();
}
