using FluentAssertions;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.UnitTests.Domain.Users;

public class UserTests
{
    [Fact]
    public void CreateUuser_ShouldBetSetName()
    {
        var user = new User("John Doe", new Email("mohammadjs2642@gmail.com"), "hashedpassword123");
        user.Username.Should().Be("John Doe");
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void CreateUser_WithoutUsername()
    {
        var user = () => new User("", new Email("mohammadjs2642@gmail.com"), "hashedpassword123");
        user.Should().Throw<ArgumentException>().WithMessage("username must be fill");
    }

    [Fact]
    public void Disable_ShouldSetIsActiveToFalse()
    {
        var user = new User("John Doe", new Email("mohammadjs2642@gmail.com"), "hashedpassword123");
        user.Disable();
        user.IsActive.Should().BeFalse();
    }

    [Fact]
    public void ChangePassword_ShouldUpdatePasswordHash()
    {
        var user = new User("John Doe", new Email("mohammadjs2642@gmail.com"), "hashedpassword123");
        user.PasswordHash.Should().Be("hashedpassword123");
        user.ChangePassword("newhashedpassword456");
        user.PasswordHash.Should().Be("newhashedpassword456");
    }

}