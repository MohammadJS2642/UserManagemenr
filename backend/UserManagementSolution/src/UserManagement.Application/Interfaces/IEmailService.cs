namespace UserManagement.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string email);
    }
}
