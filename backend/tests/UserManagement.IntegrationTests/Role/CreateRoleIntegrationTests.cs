using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Mapping;
using UserManagement.Application.UseCases.RoleUseCase;
using UserManagement.Infrastructure.Persistence;

namespace UserManagement.IntegrationTests;

public class CreateRoleIntegrationTests
{
    private readonly DbContextOptions<UserManagementDbContext> _contextOptions;
    private readonly IMapper _mapper;
    public CreateRoleIntegrationTests()
    {
        _contextOptions = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var lgf = LoggerFactory.Create(c => c.SetMinimumLevel(LogLevel.Warning));

        var config = new MapperConfiguration(cfg => cfg.AddMaps(typeof(UserProfile).Assembly), lgf);
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task Role_Should_Create_InDatabase()
    {
        using var context = new UserManagementDbContext(_contextOptions);
        var roleRepository = new RoleRepository(context);
        var uow = new UnitOfWork(context);
        var roleRequest = new CreateRoleRequests("admin");
        var command = new CreateRoleUseCase(_mapper, roleRepository, uow);

        var exec = await command.ExecuteAsync(roleRequest);

        var createdRole = await roleRepository.GetByIdAsync(exec);

        createdRole.Should().NotBeNull();
        createdRole.Name.Should().Be(roleRequest.Name);
        createdRole.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        createdRole.ModfiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        createdRole.DeletedAt.Should().BeNull();
    }

    [Fact]
    public async Task Role_Create_DuplicateName_Should_ThrowException()
    {
        using var context = new UserManagementDbContext(_contextOptions);
        var roleRepository = new RoleRepository(context);
        var uow = new UnitOfWork(context);
        var roleRequest = new CreateRoleRequests("admin");
        var command = new CreateRoleUseCase(_mapper, roleRepository, uow);
        await command.ExecuteAsync(roleRequest);
        Func<Task> act = async () => await command.ExecuteAsync(roleRequest);
        await act.Should().ThrowAsync<Exception>().WithMessage("Role name already exists.");
    }

    [Fact]
    public async Task Role_Delete_Shoule_DeleteAt_Not_Be_Null()
    {
        using var context = new UserManagementDbContext(_contextOptions);
        var roleRepository = new RoleRepository(context);
        var uow = new UnitOfWork(context);
        var roleRequest = new CreateRoleRequests("admin");
        var command = new CreateRoleUseCase(_mapper, roleRepository, uow);
        var deleteCommand = new DeleteRoleUseCase(roleRepository, uow);
        var exec = await command.ExecuteAsync(roleRequest);

        var res = await deleteCommand.ExecuteAsync(exec);

        var roleDB = await roleRepository.GetByIdAsync(exec);

        roleDB.Should().NotBeNull();
        roleDB.ModfiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        roleDB.DeletedAt.Should().NotBeNull();
        roleDB.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Role_Delete_NonExistentRole_Should_ThrowKeyNotFoundException()
    {
        using var context = new UserManagementDbContext(_contextOptions);
        var roleRepository = new RoleRepository(context);
        var uow = new UnitOfWork(context);
        var deleteCommand = new DeleteRoleUseCase(roleRepository, uow);
        Func<Task> act = async () => await deleteCommand.ExecuteAsync(999);
        await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Role with ID 999 not found.");
    }

    //TODO: Update does not exists yet
    //[Fact]
    //public async Task Update_Role_Should()
    //{
    //    using var context = new UserManagementDbContext(_contextOptions);
    //    var roleRepository = new RoleRepository(context);
    //    var uow = new UnitOfWork(context);
    //    var createCommand = new CreateRoleUseCase(_mapper, roleRepository, uow);
    //    var updateCommand = new UpdateRoleNameUseCase(_mapper, roleRepository, uow);
    //    var createRoleRequest = new CreateRoleRequests("admin");

    //    var roleId = await createCommand.ExecuteAsync(createRoleRequest);
    //    var updateRoleRequest = new UpdateRoleRequests(roleId, "superadmin");

    //    await updateCommand.ExecuteAsync(updateRoleRequest);

    //    var roleDB = await roleRepository.GetByIdAsync(roleId);
    //    roleDB.Should().NotBeNull();
    //    roleDB.Name.Should().NotBe("admin");
    //    roleDB.ModfiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    //    roleDB.DeletedAt.Should().BeNull();
    //}
}
