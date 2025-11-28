using UserManagement.Application.Interfaces;

namespace UserManagement.Application.Tests.Fakes;

internal class FakePasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        return $"hashed_{password}";
    }
}
