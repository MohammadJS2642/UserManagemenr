using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IRoleRepository
{
    Task AddAsync(Role role);
    Task<Role?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}
