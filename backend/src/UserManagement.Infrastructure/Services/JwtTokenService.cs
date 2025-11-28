using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserManagement.Application.Contracts.Auth;
using UserManagement.Application.Interfaces;

namespace UserManagement.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtSettings> settings) : IJwtTokenService
{
    public string GenerateToken(JwtUserModel jwtUser)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Value.SecretKey));
        var cerds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub,jwtUser.UserId.ToString()),
            new (JwtRegisteredClaimNames.UniqueName,jwtUser.Username),
            //new (JwtRegisteredClaimNames.GivenName,jwtUser.FirstName),
            //new (JwtRegisteredClaimNames.FamilyName,jwtUser.LastName),
            new (JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            //new (JwtRegisteredClaimNames.Jti,
            //    new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
        };

        claims.AddRange(jwtUser.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: settings.Value.Issuer,
            audience: settings.Value.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(settings.Value.ExpirationMinutes),
            signingCredentials: cerds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
