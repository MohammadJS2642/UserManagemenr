using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

public class PermissionRepository(UserManagementDbContext context) : IPermissionRepository
{
    public async Task AddAsync(Permission permission) => await context.Permissions.AddAsync(permission);

    public async Task<IEnumerable<Permission>> GetAllAsync() => await context.Permissions.ToListAsync();

    public async Task<Permission?> GetByIdAsync(int id) => await context.Permissions.FindAsync(id);

    public async Task<IEnumerable<Permission>> GetByFilterAsync(Expression<Func<Permission, bool>>? predicate = null)
    {
        IQueryable<Permission> query = context.Permissions.AsQueryable();
        if (predicate != null)
        {
            query = query.Where(predicate).AsQueryable();
        }
        return await query.ToListAsync();
    }

    public async Task Remove(int id)
    {
        var findedPermission = await GetByIdAsync(id) ??
            throw new KeyNotFoundException($"Permission with id {id} not found.");
        context.Permissions.Remove(findedPermission);
    }

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
