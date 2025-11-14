using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Application.UseCases;

public class CreateUserUseCase(IUserRepository Repo, IEmailService EmailService)
{
    public async Task<User> ExecuteAsync(string userName, string email, string password)
    {
        var emailObj = new Email(email);
        //var hash = _passwordHash.Hash(password);

        var user = new User(userName, emailObj, password);
        await Repo.AddAsync(user);
        await Repo.SaveChangesAsync();

        await EmailService.SendWelcomeEmailAsync(email);

        return user;
    }
}
