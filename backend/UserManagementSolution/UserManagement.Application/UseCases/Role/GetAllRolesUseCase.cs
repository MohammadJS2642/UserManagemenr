using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class GetAllRolesUseCase(IRoleRepository _roleRepositoyry)
{
    public async Task<IEnumerable<Role>> ExecuteAsync()
    {
        return await _roleRepositoyry.GetAllAsync();
    }
}
