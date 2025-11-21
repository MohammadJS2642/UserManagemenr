namespace UserManagement.Application.Contracts.Request;

public record CreateUserRequests(string Username, string Email, string PasswordHash);
