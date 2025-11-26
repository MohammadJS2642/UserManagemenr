using UserManagement.Application.Contracts.Response;

namespace UserManagement.Application.Interfaces;
public interface IPermissionSyncService
{
    Task<PermissionSyncResult> SyncPermissionAsync();
}

