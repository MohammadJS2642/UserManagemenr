using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

public class Role : AuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public ICollection<User> Users { get; } = new List<User>();

    private Role() { }
    public Role(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

}
