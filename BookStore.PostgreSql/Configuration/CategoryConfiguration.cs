using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Category");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(100)");
        builder.HasOne(x => x.ParentCategory).WithMany().IsRequired(false);
    }
}