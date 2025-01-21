using BookStore.Core.Model.Cart;
using BookStore.PostgreSql.Repositories;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }
    public async Task<Result<Cart>> GetCartAsync(Guid userId)
    {
        var cartResult = await _cartRepository.GetCartAsync(userId);
        if(cartResult.IsFailure)
            return Result.Failure<Cart>(cartResult.Error);
        
        return cartResult.Value;
    }
    public async Task<Result> AddBookToCartAsync(Guid userId, Guid bookId)
    {
        await _cartRepository.AddBookToCartAsync(userId, bookId);
        return Result.Success();
    }
}

public interface ICartService
{
    Task<Result<Cart>> GetCartAsync(Guid userId);
    Task<Result> AddBookToCartAsync(Guid userId, Guid bookId);
}