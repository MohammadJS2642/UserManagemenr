namespace UserManagement.Application.Contracts.Response;

public record PermissionSyncResult(
    int Added,
    int Removed,
    IReadOnlyList<string> NewPermissions,
    IReadOnlyList<string> RemovedPermissions
);
