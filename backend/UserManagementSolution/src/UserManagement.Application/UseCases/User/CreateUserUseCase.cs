using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.UseCases.User;

public class CreateUserUseCase(IUserRepository Repo/*, IEmailService EmailService*/)
{
    public async Task<UserManagement.Domain.Entities.User> ExecuteAsync(string userName, string email, string password)
    {
        var emailObj = new Email(email);
        //var hash = _passwordHash.Hash(password);

        var user = new UserManagement.Domain.Entities.User(userName, emailObj, password);
        await Repo.AddAsync(user);
        await Repo.SaveChangesAsync();

        //await EmailService.SendWelcomeEmailAsync(email);

        return user;
    }
}
