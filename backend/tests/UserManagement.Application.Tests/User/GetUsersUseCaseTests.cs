using FluentAssertions;
using UserManagement.Application.Tests.Fakes;
using UserManagement.Application.UseCases.User;
using UserManagement.Domain.ValueObjects;
using UserEntity = UserManagement.Domain.Entities.User;

namespace UserManagement.Application.Tests.User;

public class GetUsersUseCaseTests
{
    [Fact]
    public async Task Return_Users()
    {
        var userRepo = new FakeUserRepository();
        var useCase = new GetUsersUseCase(userRepo);
        var user1 = new UserEntity(1, "user1", new Email("user1@mail.com"), "123");
        var user2 = new UserEntity(2, "user2", new Email("user2@mail.com"), "123");
        var user3 = new UserEntity(3, "user3", new Email("user3@mail.com"), "123");
        var user4 = new UserEntity(4, "user4", new Email("user4@mail.com"), "123");

        await userRepo.AddAsync(user1);
        await userRepo.AddAsync(user2);
        await userRepo.AddAsync(user3);
        await userRepo.AddAsync(user4);

        var result = await useCase.ExecuteAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(4);
        result.Should().Contain(user1);
    }

    [Fact]
    public async Task Get_Users_Count_Should_Be_Zero()
    {
        var userRepo = new FakeUserRepository();
        var useCase = new GetUsersUseCase(userRepo);

        var result = await useCase.ExecuteAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }
}
