using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Cart;

public class Cart : IAggregateRoot
{
    private Cart(Guid userId, List<Book> books)
    {
        UserId = userId;
        Books = books;
    }
    
    public Guid UserId { get; private set; }
    public List<Book> Books { get; private set; }

    public Result<Price> GetTotalPrice()
    {
        if (Books.Count == 0)
        {
            return Result.Failure<Price>("The cart is empty. Total price cannot be calculated.");
        }

        decimal price = 0;
        foreach (var book in Books)
        {
            if (book.Price.Value < 0)
            {
                return Result.Failure<Price>($"Invalid price for the book: {book.Title}");
            }
            price += book.Price.Value;
        }
        
        var totalPrice = Price.Create(price);
        
        if (totalPrice.IsFailure)
            return Result.Failure<Price>(totalPrice.Error);
        
        return Result.Success(totalPrice.Value);
    }
    public static Result<Cart> Create(Guid userId, List<Book> books)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Cart>("User ID cannot be empty.");

        if (books.Count <= 0 )
            return Result.Failure<Cart>("Books list cannot be null or contain null entries.");
        
        return Result.Success(new Cart(userId, books));
    }
    public Result AddBook(Book book)
    {
        if (book.Price.Value < 0)
            return Result.Failure($"Invalid price for the book: {book.Title}");
        
        return Result.Success();
    }
}