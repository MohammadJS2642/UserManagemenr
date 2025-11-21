using AutoMapper;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Contracts.Response;
using UserManagement.Application.Interfaces;

namespace UserManagement.Application.UseCases.User;

public class CreateUserUseCase(IUnitOfWork uow, IMapper _mapper, IUserRepository repo/*, IEmailService EmailService*/)
{
    public async Task<UserResponse> ExecuteAsync(CreateUserRequests request)
    {
        //var emailObj = new Email(email);
        //var hash = _passwordHash.Hash(password);

        //var user = new UserManagement.Domain.Entities.User(userName, emailObj, password);
        var user = _mapper.Map<UserManagement.Domain.Entities.User>(request);
        await repo.AddAsync(user);
        await uow.SaveChangesAsync();

        //await EmailService.SendWelcomeEmailAsync(email);

        return _mapper.Map<UserResponse>(user);
    }
}
