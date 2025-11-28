using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserManagement.Infrastructure.Persistence;

public class UserManagementDbContextFactory : IDesignTimeDbContextFactory<UserManagementDbContext>
{
    public UserManagementDbContext CreateDbContext(string[] args)
    {
        var optionBuilder = new DbContextOptionsBuilder<UserManagementDbContext>();

        optionBuilder.UseSqlServer(
            "Server=.;Database=UserManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
        );
        return new UserManagementDbContext(optionBuilder.Options);
    }
}
