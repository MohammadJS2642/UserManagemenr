using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.User;

public class AssignRoleToUserUseCase(IUserRepository _userRepo, IRoleRepository _roleRepo, IUnitOfWork _uow)
{
    public async Task ExecuteAsync(int userId, int roleId)
    {
        var user = await _userRepo.GetByIdAsync(userId) ?? throw new Exception("User not found");
        var role = await _roleRepo.GetByIdAsync(roleId) ?? throw new Exception("Role not found");
        user.AddRole(role);
        await _uow.SaveChangesAsync();
    }
}
