using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class UpdateRoleNameUseCase(IRoleRepository _roleRepository, IUnitOfWork uow)
{
    public async Task ExecuteAsync(int roleId, string newRoleName)
    {
        var role = await _roleRepository.GetByIdAsync(roleId) ?? throw new Exception("Role not found");
        var updatedRole = new Domain.Entities.Role(newRoleName);
        typeof(Domain.Entities.Role)
            .GetProperty("Id")!
            .SetValue(updatedRole, role.Id);
        await _roleRepository.AddAsync(updatedRole);
        await uow.SaveChangesAsync();
    }
}
