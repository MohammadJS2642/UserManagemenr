using UserManagement.Application.Contracts.Auth;

namespace UserManagement.Application.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(JwtUserModel jwtUser);
}
