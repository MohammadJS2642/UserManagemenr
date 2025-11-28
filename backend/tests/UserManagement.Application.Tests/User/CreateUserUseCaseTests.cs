using FluentAssertions;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Tests.Fakes;
using UserManagement.Application.UseCases.User;

namespace UserManagement.Application.Tests.User;

public class CreateUserUseCaseTests
{
    [Fact]
    public async Task Should_Create_User_When_Request_Is_Valid()
    {
        //Arrange
        var userRepo = new FakeUserRepository();
        var passwordHasher = new FakePasswordHasher();
        var userCase = new CreateUserUseCase(userRepo, passwordHasher, new FakeUnitOfWork());

        var request = new CreateUserRequests("username", "username@mail.com", "123");

        //ACT
        var result = await userCase.ExecuteAsync(request);

        // ASSERT
        result.Should().NotBeNull();
        result.UserName.Should().Be(request.Username);
        result.Email.Should().Be(request.Email);

        userRepo.Users.Should().HaveCount(1);
        userRepo.Users.First().PasswordHash.Should().Be("123");
    }
}
