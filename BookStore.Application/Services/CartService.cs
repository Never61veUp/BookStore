using BookStore.Core.Model.Cart;
using BookStore.Core.Model.Catalog;
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
        var cartResult = await _cartRepository.GetByUserIdAsync(userId);
        if(cartResult.IsFailure)
            return Result.Failure<Cart>(cartResult.Error);
        
        return cartResult.Value;
    }

    public async Task<Result> UpdateCart(Cart cart)
    {
        return await _cartRepository.UpdateAsync(cart);
    }
    public async Task<Result> AddBookToCartAsync(Guid userId, Guid bookId)
    {
        var addResult = await _cartRepository.AddAsync(userId, bookId, 1);
        if(addResult.IsFailure)
            return Result.Failure(addResult.Error);
        return Result.Success();
    }
}

public interface ICartService
{
    Task<Result<Cart>> GetCartAsync(Guid userId);
    Task<Result> AddBookToCartAsync(Guid userId, Guid bookId);
    Task<Result> UpdateCart(Cart cart);
}