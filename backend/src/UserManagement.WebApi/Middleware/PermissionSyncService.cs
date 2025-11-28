using System.Reflection;
using UserManagement.Application.Contracts.Response;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.WebApi.Middleware;

namespace UserManagement.Infrastructure.Services;


public class PermissionSyncService(
    IPermissionRepository permissionRepository,
    IUnitOfWork uow
) : IPermissionSyncService
{
    public async Task<PermissionSyncResult> SyncPermissionAsync()
    {
        var discoveredPermissions = DiscoverPermissions();

        var dbPermissions = await permissionRepository.GetAllAsync();

        var dbCodes = dbPermissions.Select(p => p.Code).ToHashSet();
        var newCodes = discoveredPermissions.Keys.ToHashSet();

        var toAdd = newCodes.Except(dbCodes).ToList();
        var toRemove = dbCodes.Except(newCodes).ToList();

        toAdd.ForEach(async add =>
        {
            await permissionRepository.AddAsync(new Permission(add, discoveredPermissions[add]));
        });

        toRemove.ForEach(remove =>
        {
            var perm = dbPermissions.First(p => p.Code == remove);
            permissionRepository.Remove(perm.Id);
        });

        await uow.SaveChangesAsync();

        return new PermissionSyncResult(
            Added: toAdd.Count,
            Removed: toRemove.Count,
            NewPermissions: toAdd,
            RemovedPermissions: toRemove
        );
    }


    private Dictionary<string, string> DiscoverPermissions()
    {
        var assembely = Assembly.GetEntryAssembly()!;
        var controllerTypes = assembely.GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Controller"));
        //.Where(c => typeof(ControllerBase).IsAssignableFrom(c));
        var allPermissions = new Dictionary<string, string>();
        foreach (var crtl in controllerTypes)
        {
            var action = crtl.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            foreach (var method in action)
            {
                var attr = method.GetCustomAttribute<PermissionAttribute>();
                if (attr != null)
                {
                    allPermissions[attr.Code] = attr.Description ?? method.Name;
                }
            }
        }
        return allPermissions;
    }

    //public async Task SyncPermissionAsync()
    //{
    //    var assembely = Assembly.GetExecutingAssembly();
    //    var controllerTypes = assembely.GetTypes()
    //        .Where(c => typeof(ControllerBase).IsAssignableFrom(c));

    //    var allPermissions = new List<Permission>();

    //    foreach (var crtl in controllerTypes)
    //    {
    //        var action = crtl.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
    //        foreach (var a in action)
    //        {
    //            var attr = a.GetCustomAttribute<PermissionAttribute>();
    //            if (attr != null)
    //            {
    //                allPermissions.Add(new Permission(attr.Code, attr.Description));
    //            }
    //        }
    //    }

    //    var existingPermissions = await permissionRepository.GetAllAsync();
    //    foreach (var alp in allPermissions)
    //    {
    //        if (!existingPermissions.Any(e => e.Code == alp.Code))
    //        {
    //            logger.LogInformation($"Adding new permission: {alp.Code}");
    //            permissionRepository.AddAsync(new Permission(alp.Code, alp.Description)).Wait();
    //        }
    //        ;
    //    }
    //    await uow.SaveChangesAsync();
    //}

}
