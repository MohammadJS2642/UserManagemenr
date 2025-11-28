using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.Role;

public class DeleteRoleUseCase(IRoleRepository _roleRepository, IUnitOfWork uow)
{
    public async Task<bool> ExecuteAsync(int roleId)
    {
        var role = await _roleRepository.GetByIdAsync(roleId) ??
            throw new KeyNotFoundException($"Role with ID {roleId} not found.");
        role.Remove();
        var res = await uow.SaveChangesAsync();
        return res > 0;
    }
}
