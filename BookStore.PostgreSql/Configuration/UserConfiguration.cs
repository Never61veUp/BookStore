using BookStore.Core.Model.Users;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(x => x.Id);
        
        builder.ComplexProperty(s => s.Name, b =>
        {
            b.IsRequired();
            b.Property(x => x.FirstName).HasColumnName("FirstName");
            b.Property(x => x.LastName).HasColumnName("LastName");
            b.Property(x => x.MiddleName).HasColumnName("MiddleName");
        });

        builder.HasMany(x => x.Roles)
            .WithMany(u => u.Users)
            .UsingEntity<UserRoleEntity>(
                r => r.HasOne<RoleEntity>()
                    .WithMany().HasForeignKey(role => role.RoleId),
                u => u.HasOne<UserEntity>()
                    .WithMany().HasForeignKey(user => user.UserId)
            );
    }
}