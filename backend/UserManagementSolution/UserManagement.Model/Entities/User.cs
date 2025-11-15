using UserManagement.Domain.Common;
using UserManagement.Domain.ValueObjects;

namespace UserManagement.Domain.Entities;

public class User : AuditableEntity
{
    public User(string username, Email email, string passwordHash)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        IsActive = true;
    }

    public int Id { get; private set; }
    public string Username { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public bool IsActive { get; private set; }


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
}
