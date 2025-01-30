using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.HasMany<OrderItemEntity>(x => x.OrderItems)
            .WithOne(o => o.Order)
            .HasForeignKey(o => o.OrderId);
        builder.Property(x => x.OrderDate).HasColumnType("timestamp")
            .IsRequired();
    }
}