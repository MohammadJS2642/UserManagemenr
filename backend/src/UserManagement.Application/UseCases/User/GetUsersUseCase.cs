using System.Linq.Expressions;
using UserManagement.Application.Interfaces;
using UserEntity = UserManagement.Domain.Entities.User;

namespace UserManagement.Application.UseCases.User;

public class GetUsersUseCase(IUserRepository userRepository)
{
    public async Task<IEnumerable<UserEntity?>?> ExecuteAsync(
      Expression<Func<UserEntity, bool>>? predicate = null
    )
    {
        var result = await userRepository.GetUsersAsync(predicate);
        return result;
    }
}
