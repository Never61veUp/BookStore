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

    public Result<Book> ChangePrice(Price newPrice)
    {
        if(Price == newPrice)
            return Result.Failure<Book>("Price is invalid");
        
        Price = newPrice;
        return Result.Success(this);
    }
    public Result<Book> ChangeTitle(string newTitle)
    {
        if(string.IsNullOrWhiteSpace(newTitle))
            return Result.Failure<Book>("Title is required");
        if(Title == newTitle)
            return Result.Failure<Book>("Title is invalid");
        
        Title = newTitle;
        return Result.Success(this);
    }
    public Result<Book> DecreaseStockCountByOne()
    {
        if(StockCount == 0)
            return Result.Failure<Book>("Stock count is 0");
        
        StockCount--;
        return Result.Success(this);
    }
    public Result<Book> IncreaseStockCount(int count)
    {
        StockCount += count;
        return Result.Success(this);
    }
}