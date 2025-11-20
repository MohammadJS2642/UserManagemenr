using System.Linq.Expressions;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.User;

public class GetUsersUseCase(IUserRepository userRepository)
{
    public async Task<IEnumerable<UserManagement.Domain.Entities.User>> ExecuteAsync(
      Expression<Func<Domain.Entities.User, bool>>? predicate = null
    )
    {
        return await userRepository.GetUsersAsync(predicate);
    }
}
