using UserManagement.Domain.Common;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Entities;

public class User : AuditableEntity
{
    private User() { } // For EF Core
    public User(string username, Email email, string passwordHash)
    {
        CheckUsernameLenght(username);
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
    }

    private static void CheckUsernameLenght(string username)
    {
        if (username.Length == 0)
            throw new ArgumentException("username must be fill");
    }

    public int Id { get; private set; }
    public string Username { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsActive { get; private set; }

    //private readonly List<Role> _roles = [];
    //// user نباید role رو مستقیم تغییر بده
    //public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    // Business Rule
    public void Disable()
    {
        if (!IsActive)
            throw new InvalidOperationException("User is already disabled.");
        IsActive = false;
        Modify();
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("New password hash cannot be empty.", nameof(newPasswordHash));
        PasswordHash = newPasswordHash;
        Modify();
    }

    // TODO: AddRole method if needed in future
    public void AddRole(Role role)
    {
        if (_userRoles.Any(r => r.Id == role.Id))
            return;

        _userRoles.Add(new UserRole(Id, role.Id));
    }

    internal void AddRoleTest(Role role)
    {
        if (_userRoles.Any(r => r.Id == role.Id))
            return;

        _userRoles.Add(new UserRole(1, Id, role.Id));
    }

    internal User(int id, string username, Email email, string passwordHash)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
    }
}
