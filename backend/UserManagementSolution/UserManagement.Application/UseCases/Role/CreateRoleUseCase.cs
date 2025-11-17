using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class CreateRoleUseCase(IRoleRepository _roleRepository, IUnitOfWork uow)
{
    public async Task<int> ExecuteAsync(string roleName)
    {
        var getRoles = await _roleRepository.GetAllAsync();
        bool existsRoleName = getRoles.Any(x => x.Name == roleName);
        if (existsRoleName)
            throw new Exception("Role name already exists.");
        var role = new Domain.Entities.Role(roleName);
        await _roleRepository.AddAsync(role);
        await uow.SaveChangesAsync();
        return role.Id;
    }
}
