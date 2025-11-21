using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

public class RoleRepository(UserManagementDbContext _context) : IRoleRepository
{
    public async Task AddAsync(Role role) => await _context.Roles.AddAsync(role);

    public async Task<IEnumerable<Role>> GetAllAsync() => await _context.Roles.ToListAsync();

    public async Task<Role?> GetByIdAsync(int id) => await _context.Roles.FindAsync(id);

    public async Task<IEnumerable<Role>> GetByIdsAsync(Expression<Func<Role, bool>>? predict = null)
    {
        var query = _context.Roles.AsQueryable();
        if (predict != null)
        {
            query = query.Where(predict);
        }
        return await query.ToListAsync();
    }

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
