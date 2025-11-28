using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

public class UserRole : AuditableEntity
{
    public int Id { get; private set; }
    public int UserId { get; private set; }
    public User User { get; private set; } = null!;

    public int RoleId { get; private set; }
    public Role Role { get; private set; } = null!;
}
