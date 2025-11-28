using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Services;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObjects;
using UserManagement.Infrastructure.Persistence;
using RoleEntity = UserManagement.Domain.Entities.Role;
using UserEntity = UserManagement.Domain.Entities.User;

namespace UserManagement.IntegrationTests.Application;

public class AuthorizationServiceTests
{
    private readonly DbContextOptions<UserManagementDbContext> _contextOptions;

    public AuthorizationServiceTests()
    {
        _contextOptions = new DbContextOptionsBuilder<UserManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task Should_Return_False_When_User_Has_Not_Permission()
    {
        var user = new UserEntity("test", new Email("test@test.com"), "password");
        var role = new RoleEntity("admin");
        var perm = new Permission("user.create", "create user");
        using var context = new UserManagementDbContext(_contextOptions);
        var userRepository = new UserRepository(context);
        var roleRepository = new RoleRepository(context);
        var permissionRepository = new PermissionRepository(context);
        var rpRepository = new RolePermissionRepository(context);
        var authService = new AuthorizationService(userRepository, rpRepository);


        await userRepository.AddAsync(user);
        await roleRepository.AddAsync(role);
        await permissionRepository.AddAsync(perm);

        await context.SaveChangesAsync();

        var userData = (await userRepository.GetUsersAsync(u => u.Username == user.Username)).FirstOrDefault();
        var roleData = (await roleRepository.GetByIdsAsync(r => r.Name == role.Name)).FirstOrDefault();
        var permData = (await permissionRepository.GetByFilterAsync(pr => pr.Code == perm.Code)).FirstOrDefault();

        var rp = new RolePermission() { RoleId = roleData!.Id, PermissionId = permData!.Id };
        await rpRepository.AddAsync(rp);

        await context.SaveChangesAsync();


        var result = await authService.HasPermissionAsync(userData!.Id, permData!.Code);

        result.Should().BeFalse();
    }

}
