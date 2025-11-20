using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

public class Role : AuditableEntity
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<User> Users { get; } = new List<User>();

    private Role() { }
    public Role(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

    public void Remove()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
