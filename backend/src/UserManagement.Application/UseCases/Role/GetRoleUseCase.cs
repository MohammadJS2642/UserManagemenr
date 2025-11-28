using UserManagement.Application.Interfaces;
using RoleEntity=UserManagement.Domain.Entities.Role;

namespace UserManagement.Application.UseCases.Role;

public class GetRoleUseCase(IRoleRepository _roleRepository)
{
    public async Task<RoleEntity> ExecuteAsync(int roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId) ??
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");
        return role;
    }
}
