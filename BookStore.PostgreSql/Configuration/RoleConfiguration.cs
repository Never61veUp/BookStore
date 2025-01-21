using BookStore.Core.Enums;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
{
    public void Configure(EntityTypeBuilder<RoleEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasMany(r => r.GlobalPermissions)
            .WithMany(p => p.Roles)
            .UsingEntity<RolePermissionEntity>(
                l => l.HasOne<PermissionEntity>()
                    .WithMany().HasForeignKey(e => e.PermissionId),
                l => l.HasOne<RoleEntity>()
                    .WithMany().HasForeignKey(e => e.RoleId)
            );
    }
}