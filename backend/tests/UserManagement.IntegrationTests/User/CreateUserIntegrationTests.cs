using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Mapping;
using UserManagement.Application.UseCases.User;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.IntegrationTests.User;

public class CreateUserIntegrationTests
{
    private readonly DbContextOptions<UserManagementDbContext> _contextOptions;
    private readonly IMapper _mapper;
    //private readonly ILoggerFactory _loggerFactory;
    public CreateUserIntegrationTests()
    {
        _contextOptions = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var lgf = LoggerFactory.Create(builder =>
        {
            builder.SetMinimumLevel(LogLevel.Warning);
        });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(UserProfile).Assembly);
        }, lgf);
        _mapper = config.CreateMapper();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Handle_Should_CreateUser_InDatabase()
    {
        using var context = new UserManagementDbContext(_contextOptions);
        var repository = new UserRepository(context);
        var uow = new UnitOfWork(context);

        var handle = new CreateUserUseCase(uow, _mapper, repository);

        var userRequest = new CreateUserRequests("testuser", "email@email.com", "password");
        var command = await handle.ExecuteAsync(userRequest);

        var userFromDb = await repository.GetByIdAsync(command.Id);

        userFromDb.Should().NotBeNull();
        userFromDb!.Username.Should().Be("testuser");
    }

    [Fact]
    public async Task Handle_CreateAndModifyDate_Mustbe_Set()
    {
        using var context = new UserManagementDbContext(_contextOptions);
        var repositoy = new UserRepository(context);
        var uow = new UnitOfWork(context);
        var userRequest = new CreateUserRequests("testuser", "email@email.com", "password");

        var handle = new CreateUserUseCase(uow, _mapper, repositoy);

        var command = await handle.ExecuteAsync(userRequest);

        var userFromDb = await repositoy.GetByIdAsync(command.Id);

        userFromDb!.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        userFromDb!.ModfiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
