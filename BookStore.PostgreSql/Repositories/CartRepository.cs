using BookStore.Core.Model.Cart;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.PostgreSql.Repositories;

public class CartRepository : ICartRepository
{
    private readonly BookStoreDbContext _dbContext;

    public CartRepository(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Result<Cart>> GetCartAsync(Guid userId)
    {
        var booksInCard = await _dbContext.Carts.AsNoTracking().Where(x => x.UserId == userId).Select(b => b.BookId).ToListAsync();
        var bookEntities = await _dbContext.Books.AsNoTracking().Where(x => booksInCard.Contains(x.Id)).ToListAsync();
        var books = bookEntities.Select(b => Book.Create(b.Id, b.Title, b.Description, b.Price, b.AuthorId, b.CategoryId, b.StockCount).Value).ToList();
        var cart = Cart.Create(userId, books);
        if(cart.IsFailure)
            return Result.Failure<Cart>(cart.Error);
        
        return Result.Success(cart.Value);
    }

    public async Task<Result> AddBookToCartAsync(Guid userId, Guid bookId)
    {
        var book = await _dbContext.Books.AsNoTracking().FirstOrDefaultAsync(x => x.Id == bookId);
        book.StockCount -= 1;
        _dbContext.Update(book);
        CartEntity cartEntity = new()
        {
            UserId = userId,
            BookId = bookId,
        };
        await _dbContext.Carts.AddAsync(cartEntity);
        await _dbContext.SaveChangesAsync();
        return Result.Success();

    }
}

public interface ICartRepository
{
    Task<Result<Cart>> GetCartAsync(Guid userId);
    Task<Result> AddBookToCartAsync(Guid userId, Guid bookId);
}