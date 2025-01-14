using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<BookEntity>
{
    public void Configure(EntityTypeBuilder<BookEntity> builder)
    {
        builder.ToTable("Books");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title).IsRequired();
        builder.Property(x => x.Description).IsRequired();
        
        builder.ComplexProperty(s => s.Price, b =>
        {
            b.IsRequired();
            b.Property(x => x.Value).HasColumnName("Price");
        });
        
        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.StockCount).HasDefaultValue(0);
        

    }
}