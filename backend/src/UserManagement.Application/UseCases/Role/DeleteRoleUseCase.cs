using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class DeleteRoleUseCase(IRoleRepository _roleRepository, IUnitOfWork uow)
{
    public async Task ExecuteAsync(int roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId) ??
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");
        role.Remove();
        await uow.SaveChangesAsync();
    }
}
