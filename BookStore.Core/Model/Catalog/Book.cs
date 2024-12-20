using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Catalog;

public class Book : Entity<Guid>, IAggregateRoot
{
    private Book(Guid id) : base(id)
    {
        
    }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Price Price { get; private set; }
    public Guid AuthorId { get; private set; }
    public Guid CategoryId { get; private set; }
    public int StockCount { get; private set; }
    
}