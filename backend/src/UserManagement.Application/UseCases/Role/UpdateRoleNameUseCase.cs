using AutoMapper;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class UpdateRoleNameUseCase(IMapper mapper, IRoleRepository _roleRepository, IUnitOfWork uow)
{
    public async Task ExecuteAsync(UpdateRoleRequests request)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Role not found");

        var updatedRole = new Domain.Entities.Role(request.Name);
        typeof(Domain.Entities.Role)
            .GetProperty("Id")!
            .SetValue(updatedRole, role.Id);
        await _roleRepository.AddAsync(updatedRole);
        await uow.SaveChangesAsync();
    }
}
