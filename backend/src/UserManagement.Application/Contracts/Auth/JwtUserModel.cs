namespace UserManagement.Application.Contracts.Auth;

public record JwtUserModel(
    int UserId,
    string Username,
    //string FirstName,
    //string LastName,
    IEnumerable<string> Roles
);
