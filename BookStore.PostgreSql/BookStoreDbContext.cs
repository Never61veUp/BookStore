using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Configuration;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.PostgreSql;

public class BookStoreDbContext : DbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<BookEntity> Books { get; set; }
    public DbSet<AuthorEntity> Authors { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
    private ILoggerFactory CreateLoggerFactory() => LoggerFactory.Create(builder => {
        builder.AddConsole();
        builder.AddDebug();
    });
}