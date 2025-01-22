using BookStore.Application.Services;
using BookStore.Core.Model.Cart;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;
[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly AuthorizationHandlerContext _context;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet("getCard")]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }
        
        var cart = await _cartService.GetCartAsync(id);
        if(cart.IsFailure)
            return BadRequest(cart.Error);
        return Ok(cart.Value.Books);
    }
    [HttpGet("getTotalPrice")]
    public async Task<IActionResult> GetTotalPrice()
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }
        
        var cart = await _cartService.GetCartAsync(id);
        if(cart.IsFailure)
            return BadRequest(cart.Error);
        return Ok(cart.Value.GetTotalPrice().Value);
    }
    [HttpPost("addToCard")]
    public async Task<IActionResult> AddBookToCart(Guid bookId)
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }
        var cart = await _cartService.AddBookToCartAsync(id, bookId);
        if(cart.IsFailure)
            return BadRequest(cart.Error);
        return Ok("Успешно");
    }
}