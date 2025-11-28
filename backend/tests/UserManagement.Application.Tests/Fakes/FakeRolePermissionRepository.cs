using System.Linq.Expressions;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Tests.Fakes;

internal class FakeRolePermissionRepository : IRolePermissionRepository
{
    public List<RolePermission> RolePermissions { get; } = new List<RolePermission>();
    public Task AddAsync(RolePermission rolePermission)
    {
        RolePermissions.Add(rolePermission);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<RolePermission>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<RolePermission>> GetByFilterAsync(Expression<Func<RolePermission, bool>>? predict = null)
    {
        return Task.FromResult(RolePermissions.AsQueryable().Where(predict!).AsEnumerable());
    }

    public Task<RolePermission?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}
