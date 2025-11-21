using AutoMapper;
using System.Linq.Expressions;
using UserManagement.Application.Contracts.Response;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.User;

public class GetUsersUseCase(IMapper _mapper, IUserRepository userRepository)
{
    public async Task<IEnumerable<UserResponse>> ExecuteAsync(
      Expression<Func<Domain.Entities.User, bool>>? predicate = null
    )
    {
        var result = await userRepository.GetUsersAsync(predicate);
        return _mapper.Map<IEnumerable<UserResponse>>(result);
    }
}
