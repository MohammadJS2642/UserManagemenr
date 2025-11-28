using System.Linq.Expressions;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IPermissionRepository
{
    Task<IEnumerable<Permission>> GetByFilterAsync(Expression<Func<Permission, bool>>? predicate = null);
    Task AddAsync(Permission permission);
    Task<Permission?> GetByIdAsync(int id);
    Task<IEnumerable<Permission>> GetAllAsync();
    Task Remove(int id);
    Task SaveChangesAsync();
}
