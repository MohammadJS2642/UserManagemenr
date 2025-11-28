using FluentAssertions;
using UserManagement.Application.Tests.Fakes;
using UserManagement.Application.UseCases.User;
using UserManagement.Domain.ValueObjects;
using UserEntity = UserManagement.Domain.Entities.User;

namespace UserManagement.Application.Tests.User;

public class DisableUserUseCaseTests
{
    [Fact]
    public async Task Disable_User_Should_Be_False_Modified_Changed()
    {
        // Arrange
        var uow = new FakeUnitOfWork();
        var userRepo = new FakeUserRepository();
        var useCase = new DisableUserUseCase(uow, userRepo);
        var user = new UserEntity(1, "username", new Email("username@mail.com"), "hash");

        await userRepo.AddAsync(user);

        // Act
        await useCase.ExecuteAsync(user.Id);

        // Assert
        user.IsActive.Should().BeFalse();
        user.ModfiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task User_Not_Found()
    {
        // Arrange
        var uow = new FakeUnitOfWork();
        var userRepo = new FakeUserRepository();
        var useCase = new DisableUserUseCase(uow, userRepo);
        var user = new UserEntity(1, "username", new Email("username@mail.com"), "hash");

        await userRepo.AddAsync(user);

        // Act
        Func<Task> act = async () => await useCase.ExecuteAsync(2);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("*User not found*");
    }

}
