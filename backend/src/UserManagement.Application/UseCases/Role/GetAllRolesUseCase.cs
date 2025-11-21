using AutoMapper;
using UserManagement.Application.Contracts.Response;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.RoleUseCase;

public class GetAllRolesUseCase(IMapper _mapper, IRoleRepository _roleRepositoyry)
{
    public async Task<IEnumerable<RoleResponse>> ExecuteAsync()
    {
        var results = await _roleRepositoyry.GetAllAsync();
        return _mapper.Map<IEnumerable<RoleResponse>>(results);
    }
}
