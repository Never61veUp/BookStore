using BookStore.Core.Model.Cart;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Repositories;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Services;

public interface IOrderService
{
    Task<Result> CreateOrder(Guid userId, Dictionary<Book, int> books);
    Task<Result<IEnumerable<Order>>> GetOrdersByIdAsync(Guid userId);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> CreateOrder(Guid userId, Dictionary<Book, int> books)
    {
        var order = Order.Create(userId, books, OrderStatus.Pending, DateTime.Now);
        if(order.IsFailure)
            return Result.Failure<Order>(order.Error);
        
        var orderTask = await _orderRepository.CreateOrder(order.Value);
        return orderTask;
    }

    public async Task<Result<IEnumerable<Order>>> GetOrdersByIdAsync(Guid userId)
    {
        return await _orderRepository.GetOrdersByUserIdAsync(userId);
    }
}