using AutoMapper;
using BookStore.Core.Model.Cart;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
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

    public async Task<Result<Cart>> GetByUserIdAsync(Guid userId)
    {
        var cartEntity = await _dbContext.Carts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cartEntity is null)
        {
            return Result.Failure<Cart>("Cart not found");
        }

        var cartItemEntities = await _dbContext.CartItems.AsNoTracking()
            .Where(x => x.CartId == cartEntity.Id)
            .ToListAsync();

        var bookIds = cartItemEntities.Select(x => x.BookId).ToList();
        var bookEntitiesById = await GetBooksByIdsAsync(bookIds);

        var bookResults = MapCartItemsToBooks(cartItemEntities, bookEntitiesById);

        if (bookResults.Any(r => r.IsFailure))
        {
            var firstError = bookResults.First(r => r.IsFailure).Error;
            return Result.Failure<Cart>(firstError);
        }

        var bookDictionary = bookResults
            .Select(r => r.Value)
            .ToDictionary(x => x.Item1, x => x.Item2);

        var cart = Cart.Create(userId, bookDictionary);

        return Result.Success(cart.Value);
    }

    public async Task<Result> AddAsync(Guid userId, Guid bookId, int quantity)
    {
        if (quantity <= 0)
        {
            return Result.Failure("Quantity must be greater than 0");
        }

        var cartEntity = await _dbContext.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId) ?? new CartEntity(Guid.NewGuid())
        {
            UserId = userId
        };

        var bookEntity = await _dbContext.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (bookEntity is null)
        {
            return Result.Failure("Book not found");
        }

        var cartItem = await _dbContext.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartEntity.Id && ci.BookId == bookId);

        if (cartItem is null)
        {
            cartItem = new CartItemEntity(Guid.NewGuid())
            {
                CartId = cartEntity.Id,
                BookId = bookId,
                Quantity = quantity
            };
            _dbContext.CartItems.Add(cartItem);
        }
        else
        {
            cartItem.Quantity += quantity;
        }

        await _dbContext.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(Cart cart)
    {
        var existingCart = await _dbContext.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == cart.UserId);

        if (existingCart == null)
        {
            existingCart = new CartEntity(Guid.NewGuid())
            {
                UserId = cart.UserId,
                Items = new List<CartItemEntity>()
            };
        }

        _dbContext.CartItems.RemoveRange(existingCart.Items);

        var cartItems = cart.Books.Select(x => new CartItemEntity(Guid.NewGuid())
        {
            Cart = existingCart,
            CartId = existingCart.Id,
            Quantity = x.Value,
            BookId = x.Key.Id
        }).ToList();

        await _dbContext.CartItems.AddRangeAsync(cartItems);

        var result = await _dbContext.SaveChangesAsync() > 0
            ? Result.Success()
            : Result.Failure("Cart could not be updated.");

        return result;
    }

    private List<Result<(Book, int)>> MapCartItemsToBooks(
        IEnumerable<CartItemEntity> cartItems,
        Dictionary<Guid, BookEntity> bookEntitiesById)
    {
        return cartItems.Select(item =>
        {
            if (!bookEntitiesById.TryGetValue(item.BookId, out var bookEntity))
            {
                return Result.Failure<(Book, int)>($"Book with ID {item.BookId} not found");
            }

            var bookResult = _mapper.Map<Book>(bookEntity);
            return Result.Success((bookResult, item.Quantity));
        }).ToList();
    }

    private async Task<Dictionary<Guid, BookEntity>> GetBooksByIdsAsync(IEnumerable<Guid> bookIds)
    {
        var bookEntities = await _dbContext.Books.AsNoTracking()
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => bookIds.Contains(b.Id))
            .ToListAsync();

        return bookEntities.ToDictionary(b => b.Id, b => b);
    }
}

public interface ICartRepository
{
    Task<Result<Cart>> GetByUserIdAsync(Guid userId);
    Task<Result> AddAsync(Guid userId, Guid bookId, int quantity);
    Task<Result> UpdateAsync(Cart cart);
}