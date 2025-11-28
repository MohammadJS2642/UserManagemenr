using FluentAssertions;
using UserManagement.Application.Services;
using UserManagement.Application.Tests.Fakes;
using UserManagement.Domain.Entities;
using UserManagement.Domain.ValueObjects;
using UserEntity = UserManagement.Domain.Entities.User;

namespace UserManagement.Application.Tests.Auth;

public class AuthorizationServiceTests
{
    [Fact]
    public async Task Should_Return_True_When_User_Has_Permission()
    {
        //Arrange
        var userRepo = new FakeUserRepository();
        var rolePermissionRepo = new FakeRolePermissionRepository();

        var user = new UserEntity(1, "username", new Email("username@mail.com"), "hash");
        var role = new Role(1, "admin");
        var permission = new Permission(1, "user.create", "create user");

        user.AddRoleTest(role);

        await userRepo.AddAsync(user);

        await rolePermissionRepo.AddAsync(new RolePermission(role.Id, permission.Id) { Permission = permission });

        var service = new AuthorizationService(userRepo, rolePermissionRepo);

        // ACT
        var result = await service.HasPermissionAsync(user.Id, permission.Code);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Return_False_When_User_Does_Not_Have_Permission()
    {
        // Arrange
        var userRepo = new FakeUserRepository();
        var rolePermissionRepo = new FakeRolePermissionRepository();

        var user = new UserEntity(
            "reza",
            new Email("reza@test.com"),
            "HASHED-password"
        );
        user.AddRole(new Role("admin"));

        var getUserRole = await userRepo.GetUserByRoles(u => u.Username == user.Username);

        // نقش 2 هیچ permission ندارد

        var service = new AuthorizationService(userRepo, rolePermissionRepo);

        // Act
        var result = await service.HasPermissionAsync(user.Id, "USER_DELETE");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Return_False_When_User_Is_Not_Active()
    {
        // Arrange
        var userRepo = new FakeUserRepository();
        var rolePermissionRepo = new FakeRolePermissionRepository();

        var user = new UserEntity(
            "karim",
            new Email("karim@test.com"),
            "HASHED"
        );

        user.Disable();

        user.AddRole(new Role("admin"));

        userRepo.Users.Add(user);

        var service = new AuthorizationService(userRepo, rolePermissionRepo);

        // Act
        var result = await service.HasPermissionAsync(user.Id, "ANY");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Should_Return_False_When_User_Not_Found()
    {
        // Arrange
        var service = new AuthorizationService(
            new FakeUserRepository(),
            new FakeRolePermissionRepository()
        );

        // Act
        var result = await service.HasPermissionAsync(999, "ANY");

        // Assert
        result.Should().BeFalse();
    }
}
