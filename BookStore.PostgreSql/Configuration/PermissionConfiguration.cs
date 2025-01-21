using BookStore.Core.Enums;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
{
    public void Configure(EntityTypeBuilder<PermissionEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(r => r.Roles)
            .WithMany(p => p.GlobalPermissions)
            .UsingEntity<RolePermissionEntity>(
                l => l.HasOne<RoleEntity>()
                    .WithMany().HasForeignKey(e => e.RoleId),
                l => l.HasOne<PermissionEntity>()
                    .WithMany().HasForeignKey(e => e.PermissionId)
            );
    }
}