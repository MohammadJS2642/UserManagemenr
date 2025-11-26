using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

public class RolePermission : AuditableEntity
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public Role Role { get; set; } = default!;
    public Permission Permission { get; set; } = default!;
}
