using UserManagement.Application.Contracts.Auth;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services;

public class AuthService(IUserRepository userRepo, IJwtTokenService jwtTokenService) : IAuthService
{
    public async Task<string> LoginAsync(string username, string password)
    {

        var user = await userRepo.GetUserByRoles(u => u.Username == username && u.IsActive);
        //TODO: Verify Password Hash
        if (user == null)
            throw new UnauthorizedAccessException("Invalid Credentials");

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
        var jwtTokenModel = new JwtUserModel
        (
            user.Id,
            user.Username,
            //user.FirstName,
            //user.LastName,
            roles
        );

        return jwtTokenService.GenerateToken(jwtTokenModel);
    }
}
