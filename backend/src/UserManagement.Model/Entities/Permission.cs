using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

public class Permission : AuditableEntity
{
    public int Id { get; private set; }
    public string Code { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    //public ICollection<Role> Roles { get; } = [];
    public ICollection<RolePermission> RolePermissions { get; set; }


    private Permission() { }
    public Permission(string code, string description)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }
    internal Permission(int id, string code, string description)
    {
        Id = id;
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }
}
