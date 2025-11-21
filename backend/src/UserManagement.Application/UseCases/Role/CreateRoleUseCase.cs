using AutoMapper;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class CreateRoleUseCase(IMapper mapper, IRoleRepository _roleRepository, IUnitOfWork uow)
{
    public async Task<int> ExecuteAsync(CreateRoleRequests requests)
    {
        var getRoles = await _roleRepository.GetAllAsync();
        bool existsRoleName = getRoles.Any(x => x.Name == requests.Name);
        if (existsRoleName)
            throw new Exception("Role name already exists.");
        var roleName = mapper.Map<Role>(requests);
        var role = new Domain.Entities.Role(roleName.Name);
        await _roleRepository.AddAsync(role);
        await uow.SaveChangesAsync();
        return role.Id;
    }
}
