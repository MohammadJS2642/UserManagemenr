using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

public class RoleRepository(UserManagementDbContext _context) : IRoleRepository
{
    public async Task AddAsync(Role role) => await _context.Roles.AddAsync(role);

    public async Task<Role?> GetByIdAsync(int id) => await _context.Roles.FindAsync(id);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}
