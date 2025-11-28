using UserManagement.Domain.Common;

namespace UserManagement.Domain.Entities;

public class RolePermission : AuditableEntity
{
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    public Role Role { get; set; } = default!;
    public Permission Permission { get; set; } = default!;

    private RolePermission() { }
    public RolePermission(int roleId, int permissionId)
    {
        RoleId = roleId;
        PermissionId = permissionId;
    }
    public RolePermission(int roleId, Permission permission)
    {
        RoleId = roleId;
        PermissionId = permission.Id;
    }
}
