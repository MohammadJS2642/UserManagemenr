using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infrastructure.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                 .HasColumnName("Email")
                 .IsRequired();
        });
        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();

        builder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));
    }
}
