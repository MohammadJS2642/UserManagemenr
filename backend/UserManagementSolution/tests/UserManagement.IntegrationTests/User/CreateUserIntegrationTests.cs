using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.UseCases.User;
using UserManagement.Domain.Entities;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.IntegrationTests.User;

public class CreateUserIntegrationTests
{
    private readonly DbContextOptions<UserManagementDbContext> _contextOptions;
    public CreateUserIntegrationTests()
    {
        _contextOptions = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Handle_Should_CreateUser_InDatabase()
    {
        using var context = new UserManagementDbContext(_contextOptions);
        var repository = new UserRepository(context);
        var uow = new UnitOfWork(context);

        var handle = new CreateUserUseCase(uow, repository);

        var command = await handle.ExecuteAsync("testuser", "email@email.com", "password");

        var userFromDb = await repository.GetByIdAsync(command.Id);

        userFromDb.Should().NotBeNull();
        userFromDb!.Username.Should().Be("testuser");
    }
}
