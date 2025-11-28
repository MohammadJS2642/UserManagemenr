using System.Linq.Expressions;
using UserManagement.Application.Interfaces;
using UserEntity = UserManagement.Domain.Entities.User;

namespace UserManagement.Application.Tests.Fakes;

internal class FakeUserRepository : IUserRepository
{
    public List<UserEntity> Users { get; } = new List<UserEntity>();

    public Task AddAsync(Domain.Entities.User user)
    {
        Users.Add(user);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Domain.Entities.User>> GetAllAsync()
    {
        return Task.FromResult(Users.AsEnumerable());
    }

    public Task<Domain.Entities.User?> GetByEmailAsync(string email)
    {
        return Task.FromResult(Users.FirstOrDefault(x => x.Email.Value == email));
    }

    public Task<Domain.Entities.User?> GetByIdAsync(int id)
    {
        return Task.FromResult(Users.FirstOrDefault(x => x.Id == id));
    }

    public Task<Domain.Entities.User?> GetUserByRoles(Expression<Func<Domain.Entities.User, bool>> predicate)
    {
        return Task.FromResult(Users.AsQueryable().FirstOrDefault(predicate));
    }

    public Task<IEnumerable<Domain.Entities.User>> GetUsersAsync(Expression<Func<Domain.Entities.User, bool>>? predicate = null)
    {
        return Task.FromResult(Users.AsEnumerable());
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}
