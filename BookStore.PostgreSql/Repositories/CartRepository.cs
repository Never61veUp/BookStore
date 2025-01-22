using AutoMapper;
using BookStore.Core.Model.Cart;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.PostgreSql.Repositories;

public class CartRepository : ICartRepository
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CartRepository(BookStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<Result<Cart>> GetCartAsync(Guid userId)
    {
        var booksInCard = await _dbContext.Carts.AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(b => b.BookId).ToListAsync();
        
        var bookEntities = await _dbContext.Books.AsNoTracking()
            .Where(x => booksInCard.Contains(x.Id))
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();
        var books = _mapper.Map<List<Book>>(bookEntities);
        var bookDictionary = bookEntities
            .Select(b => new
            {
                Book = books.First(x => x.Id == b.Id),
                Count = booksInCard.Count(ci => ci == b.Id)
            })
            .Where(x => x.Book != null && x.Count > 0)
            .ToDictionary(x => x.Book, x => x.Count);
        
        var cart = Cart.Create(userId, bookDictionary);
        if(cart.IsFailure)
            return Result.Failure<Cart>(cart.Error);
        
        return Result.Success(cart.Value);
        return Result.Failure<Cart>("ds");
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