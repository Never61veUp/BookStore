using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Quantity).IsRequired();
        
        builder.HasOne(x => x.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(x => x.OrderId);
    }
}