using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

public class RolePermissionRepository(UserManagementDbContext context) : IRolePermissionRepository
{
    public async Task AddAsync(RolePermission rolePermission) => await context.RolePermissions.AddRangeAsync(rolePermission);

    public async Task<IEnumerable<RolePermission>> GetAllAsync() => await context.RolePermissions.ToListAsync();

    public async Task<IEnumerable<RolePermission>> GetByFilterAsync(Expression<Func<RolePermission, bool>>? predict = null)
    {
        IQueryable<RolePermission> query = context.RolePermissions.AsQueryable();
        if (predict != null)
        {
            query = query.Where(predict).AsQueryable();
        }
        return await query.ToListAsync();
    }

    public async Task<RolePermission?> GetByIdAsync(int id) => await context.RolePermissions.FindAsync(id);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
}
