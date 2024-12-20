using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Catalog;

public class Book : Entity<Guid>, IAggregateRoot
{
    private Book(Guid id, string title, string description, Price price, Guid authorId, Guid categoryId, int stockCount) : base(id)
    {
        Title = title;
        Description = description;
        Price = price;
        AuthorId = authorId;
        CategoryId = categoryId;
        StockCount = stockCount;
    }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Price Price { get; private set; }
    public Guid AuthorId { get; private set; }
    public Guid CategoryId { get; private set; }
    public int StockCount { get; private set; }

    public static Result<Book> Create(Guid id, string title, string description, Price price, Guid authorId,
        Guid categoryId, int stockCount)
    {
        if(string.IsNullOrWhiteSpace(title))
            return Result.Failure<Book>("Title is required");
        if(string.IsNullOrWhiteSpace(description))
            return Result.Failure<Book>("Description is required");
        if(authorId == Guid.Empty)
            return Result.Failure<Book>("AuthorId is required");
        if(categoryId == Guid.Empty)
            return Result.Failure<Book>("CategoryId is required");
        
        if(id == Guid.Empty)
            id = Guid.NewGuid();

        return Result.Success(
            new Book(id, title, description, price, authorId, categoryId, stockCount));
    }
    
}