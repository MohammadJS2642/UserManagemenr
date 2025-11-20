using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.UseCases.User;

public class CreateUserUseCase(IUnitOfWork uow, IUserRepository repo/*, IEmailService EmailService*/)
{
    public async Task<UserManagement.Domain.Entities.User> ExecuteAsync(string userName, string email, string password)
    {
        var emailObj = new Email(email);
        //var hash = _passwordHash.Hash(password);

        var user = new UserManagement.Domain.Entities.User(userName, emailObj, password);
        await repo.AddAsync(user);
        await uow.SaveChangesAsync();

        //await EmailService.SendWelcomeEmailAsync(email);

        return user;
    }
}
