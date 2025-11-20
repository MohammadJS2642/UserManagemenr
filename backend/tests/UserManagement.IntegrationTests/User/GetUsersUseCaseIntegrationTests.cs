using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.UseCases.User;
using UserManagement.Domain.ValueObjects;
using UserManagement.Infrastructure.Persistence;
using userDomain = UserManagement.Domain.Entities.User;

namespace UserManagement.IntegrationTests.User;

public class GetUsersUseCaseIntegrationTests
{
    private readonly DbContextOptions<UserManagementDbContext> _options;

    public GetUsersUseCaseIntegrationTests()
    {
        _options = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    private async Task SeedDatasAsync()
    {
        using var context = new UserManagementDbContext(_options);
        context.Users.AddRange(new[]
        {
            new userDomain("user1",new Email("email1@test.com"),"password1"),
            new userDomain("user2",new Email("email2@test.com"),"password2"),
            new userDomain("user3",new Email("email3@test.com"),"password3"),
            new userDomain("user4",new Email("email4@test.com"),"password4"),
            new userDomain("user5",new Email("email5@test.com"),"password5")
        });
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task Should_Return_All_Users_When_No_Filter()
    {
        await SeedDatasAsync();
        using var context = new UserManagementDbContext(_options);

        var repo = new UserRepository(context);
        var useCase = new GetUsersUseCase(repo);

        var result = await useCase.ExecuteAsync();

        result.Should().HaveCount(5);
    }

    [Fact]
    public async Task Should_Return_Filtered_Users_By_Id()
    {
        await SeedDatasAsync();
        using var context = new UserManagementDbContext(_options);
        var repo = new UserRepository(context);
        var useCase = new GetUsersUseCase(repo);
        var result = await useCase.ExecuteAsync(u => u.Id == 3);
        result.Should().HaveCount(1);
        result.All(u => u.Id == 3).Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Filtered_Users_By_Username()
    {
        await SeedDatasAsync();
        using var context = new UserManagementDbContext(_options);
        var repo = new UserRepository(context);
        var useCase = new GetUsersUseCase(repo);
        var result = await useCase.ExecuteAsync(u => u.Username == "user4");
        result.Should().HaveCount(1);
        result.All(u => u.Username == "user4").Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_Empty_When_No_Match()
    {
        await SeedDatasAsync();
        using var context = new UserManagementDbContext(_options);
        var repo = new UserRepository(context);
        var useCase = new GetUsersUseCase(repo);
        var result = await useCase.ExecuteAsync(u => u.Username == "nonexistentuser");
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_Handle_Multiple_Filters_Correctly()
    {
        await SeedDatasAsync();
        using var context = new UserManagementDbContext(_options);
        var repo = new UserRepository(context);
        var useCase = new GetUsersUseCase(repo);
        var result = await useCase.ExecuteAsync(u => u.Id == 2 && u.Username.Contains("user"));
        result.Should().HaveCount(1);
        result.All(u => u.Id == 2).Should().BeTrue();
    }
}
