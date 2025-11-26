using System.Linq.Expressions;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IRolePermissionRepository
{
    Task AddAsync(RolePermission rolePermission);
    Task<IEnumerable<RolePermission>> GetAllAsync();
    Task<RolePermission?> GetByIdAsync(int id);
    Task<IEnumerable<RolePermission>> GetByFilterAsync(Expression<Func<RolePermission, bool>>? predict = null);
    Task SaveChangesAsync();
}
