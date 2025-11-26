using Microsoft.EntityFrameworkCore;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence;

public class UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive == true);
        modelBuilder.Entity<Role>().HasQueryFilter(r => r.DeletedAt == null);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserManagementDbContext).Assembly);
    }
}
