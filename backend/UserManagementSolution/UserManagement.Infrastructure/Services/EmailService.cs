using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services;

public class EmailService : IEmailService
{
    public Task SendWelcomeEmailAsync(string email)
    {
        throw new NotImplementedException();
    }
}
