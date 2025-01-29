using BookStore.Core.Model.Cart;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Repositories;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Services;

public interface IOrderService
{
    Task<Result> CreateOrder(Guid userId, Cart cart);
    Task<Result<IEnumerable<Order>>> GetOrdersByIdAsync(Guid userId);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;

    public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
    }

    public async Task<Result> CreateOrder(Guid userId, Cart cart)
    {
        var order = Order.Create(userId, cart.Books, OrderStatus.Pending, DateTime.Now, ref cart);
        if(order.IsFailure)
            return Result.Failure<Order>(order.Error);
        
        var orderTask = await _orderRepository.CreateOrder(order.Value);
        await _cartRepository.UpdateAsync(cart);
        return orderTask;
    }

    public async Task<Result<IEnumerable<Order>>> GetOrdersByIdAsync(Guid userId)
    {
        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }
}