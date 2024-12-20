using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.PostgreSql.Configuration;

public class AuthorConfiguration : IEntityTypeConfiguration<AuthorEntity>
{
    public void Configure(EntityTypeBuilder<AuthorEntity> builder)
    {
        builder.ToTable("Author");
        
        builder.HasKey(x => x.Id);
        
        builder.ComplexProperty(s => s.FullName, b =>
        {
            b.IsRequired();
            b.Property(x => x.FirstName).HasColumnName("FirstName");
            b.Property(x => x.LastName).HasColumnName("LastName");
            b.Property(x => x.MiddleName).HasColumnName("MiddleName");
        });
        
        builder.Property(s => s.BirthDate).HasColumnName("BirthDate").HasColumnType("date");
        builder.Property(x => x.Biography).HasColumnName("Biography").HasColumnType("text");
        
        
    }
}