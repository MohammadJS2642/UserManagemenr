using FluentAssertions;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.UnitTests.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("user@gmail.com")]
    [InlineData("text@example.com")]
    public void CreateEmail_WithValidValue_ShouldSetValue(string emailString)
    {
        // Act
        var email = new Email(emailString);
        // Assert
        email.Value.Should().Be(emailString);
        email.ToString().Should().Be(emailString);
    }

    [Theory]
    [InlineData("")]
    [InlineData("       ")]
    [InlineData(null)]
    [InlineData("invalid.com")]
    public void CreateEmail_WithInvalidValue_ShouldThrowArgumentException(string? emailString)
    {
        // Act
        var email = () => new Email(emailString!);
        // Assert
        email.Should().Throw<ArgumentException>().WithMessage("*Invalid email address*");
    }

    [Fact]
    public void Email_ShouldBe_Immmutable()
    {
        var email = new Email("test@gmail.com");
        typeof(Email).GetProperty(nameof(email.Value))!.CanWrite.Should().BeFalse();
    }
}
