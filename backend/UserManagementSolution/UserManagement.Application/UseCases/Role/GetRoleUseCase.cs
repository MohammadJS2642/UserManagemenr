using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class GetRoleUseCase(IRoleRepository _roleRepository)
{
    public async Task<Role> ExecuteAsync(int roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId) ??
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");
        return role;
    }
}
