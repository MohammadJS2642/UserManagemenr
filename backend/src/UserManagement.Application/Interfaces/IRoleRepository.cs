using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces;

public interface IRoleRepository
{
    Task AddAsync(Role role);
    Task<IEnumerable<Role>> GetAllAsync();
    Task<Role?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}
