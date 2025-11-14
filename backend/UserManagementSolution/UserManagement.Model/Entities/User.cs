namespace UserManagement.Domain.Entities;

public class User
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

}
