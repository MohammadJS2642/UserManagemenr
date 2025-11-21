using FluentAssertions;

namespace UserManagement.UnitTests.Domain.Role;

public class RoleTests
{
    [Fact]
    public void Role_Should_Be_Created_Correctly()
    {
        var role = new UserManagement.Domain.Entities.Role("Admin");

        role.Name.Should().Be("Admin");
        role.DeletedAt.Should().BeNull();
    }

    [Fact]
    public void Role_CreatedAt_Should_Be_Set_On_Creation()
    {
        var role = new UserManagement.Domain.Entities.Role("User");
        role.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Role_Should_Mark_As_Deleted_Correctly()
    {
        var role = new UserManagement.Domain.Entities.Role("Moderator");
        role.Remove();
        role.DeletedAt.Should().NotBeNull();
        role.DeletedAt.Value.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }
}