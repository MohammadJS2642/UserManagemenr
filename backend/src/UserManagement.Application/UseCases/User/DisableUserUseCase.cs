using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.User;

public class DisableUserUseCase(IUnitOfWork uow, IUserRepository _userRepository)
{
    public async Task<bool> ExecuteAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new Exception("User not found");
        user.Disable();
        var result = await uow.SaveChangesAsync();
        return result > 0;
    }
}
