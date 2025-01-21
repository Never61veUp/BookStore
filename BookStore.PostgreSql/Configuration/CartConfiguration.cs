using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class CartConfiguration : IEntityTypeConfiguration<CartEntity>
{
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.HasKey(x => new { x.UserId, x.BookId });
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.BookId).IsRequired();
        builder.ToTable("Cart");
    }
}