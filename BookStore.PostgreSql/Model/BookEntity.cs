using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class BookEntity : Entity<Guid>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Price Price { get; set; }
    public AuthorEntity Author { get; set; }
    public CategoryEntity Category { get; set; }
    public int StockCount { get; set; }
}