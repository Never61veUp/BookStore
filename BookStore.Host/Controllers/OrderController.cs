using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ICartService _cartService;

    public OrderController(IOrderService orderService, ICartService cartService)
    {
        _orderService = orderService;
        _cartService = cartService;
    }
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();
        
        var result = await _orderService.GetOrdersByIdAsync(userId);
        if(result.IsFailure)
            return BadRequest(result.Error);
        return Ok(result.Value);
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {
        if (!TryGetUserId(out var userId))
            return Unauthorized();
        
        var cart =  await _cartService.GetCartAsync(userId);
        if (cart.IsFailure)
            return BadRequest(cart.Error);
        
        var result = await _orderService.CreateOrder(userId, cart.Value.Books);
        if(result.IsFailure)
            return BadRequest(result.Error);
        return Ok("Order created");
    }
    
    private bool TryGetUserId(out Guid id)
    {
        id = Guid.Empty;
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out id))
            return false;

        return true;
    }
}