using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Contracts.Response;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.User;

public class CreateUserUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork uow/*, IEmailService EmailService*/)
{
    public async Task<UserResponse> ExecuteAsync(CreateUserRequests request)
    {
        // Rule: email must be unique
        var existing = await userRepository.GetByEmailAsync(request.Email);
        if (existing is not null)
            throw new Exception("Email is already in use.");

        // Rule: password must be hashed
        //var hashed = passwordHasher.GetHashCode(request.PasswordHash);

        var user = new Domain.Entities.User(request.Username, new Domain.ValueObjects.Email(request.Email), request.PasswordHash);

        await userRepository.AddAsync(user);
        await uow.SaveChangesAsync();

        //await EmailService.SendWelcomeEmailAsync(email);

        return new UserResponse(user.Id, user.Username, user.Email.Value);
    }
}
