using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

public class Role : AuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int CreatedBy { get; set; }
    public int? ModifiedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public ICollection<UserRole> UserRoles { get; set; }

    //public ICollection<User> Users { get; } = new List<User>();

    //private readonly List<Permission> _permissions = [];
    //public IReadOnlyCollection<Permission> Permissions => _permissions.AsReadOnly();


    public ICollection<RolePermission> RolePermissions { get; set; }


    private Role() { }
    public Role(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

    internal Role(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public void Remove()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
