using AutoMapper;
using BookStore.Core.Model.Cart;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.PostgreSql.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public OrderRepository(BookStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<Order>>> GetOrdersByUserIdAsync(Guid userId)
    {
        var orderEntities = await _dbContext.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ToListAsync();
        if (orderEntities.Count == 0)
        {
            return Result.Success<IEnumerable<Order>>(new List<Order>());
        }
        
        var orderItems = orderEntities.SelectMany(o => o.OrderItems).ToList();
        var bookIds = orderItems.Select(x => x.BookId).Distinct().ToList();
        var bookEntitiesById = await GetBooksByIdsAsync(bookIds);

        var orders = orderEntities
            .Select(entity =>
            {
                var orderItemEntities = entity.OrderItems;

                var bookResultsForOrder = MapCartItemsToBooks(orderItemEntities, bookEntitiesById);

                if (bookResultsForOrder.Any(r => r.IsFailure))
                {
                    var firstError = bookResultsForOrder.First(r => r.IsFailure).Error;
                    return Result.Failure<Order>(firstError);
                }

                var bookDictionaryForOrder = bookResultsForOrder
                    .Select(r => r.Value)
                    .GroupBy(x => x.Item1)
                    .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2));

                return Order.Create(
                    entity.UserId,
                    bookDictionaryForOrder,
                    entity.OrderStatus,
                    entity.OrderDate);
            })
            .ToList();

        if (orders.Any(r => r.IsFailure))
        {
            var firstError = orders.First(r => r.IsFailure).Error;
            return Result.Failure<IEnumerable<Order>>(firstError);
        }

        return Result.Success(orders.Select(r => r.Value));
    }

    public async Task<Result> CreateOrder(Order order)
    {
        var orderEntity = new OrderEntity(Guid.NewGuid())
        {
            UserId = order.UserId,
            OrderStatus = order.OrderStatus,
            OrderDate = order.OrderDate,
        };
        var orderItems = order.Books.Select(x => new OrderItemEntity(Guid.NewGuid())
        {
            BookId = x.Key.Id,
            Order = orderEntity,
            Quantity = x.Value
        }).ToList();
        orderEntity.OrderItems = orderItems;
        _dbContext.Orders.Add(orderEntity);
        await _dbContext.OrderItems.AddRangeAsync(orderItems);
        var result = await _dbContext.SaveChangesAsync() > 0
            ? Result.Success("Success")
            : Result.Failure("Cart could not be updated.");

        return result;
        
    }
    private List<Result<(Book, int)>> MapCartItemsToBooks(
        IEnumerable<OrderItemEntity> orderItems,
        Dictionary<Guid, BookEntity> bookEntitiesById)
    {
        return orderItems.Select(item =>
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

public interface IOrderRepository
{
    Task<Result<IEnumerable<Order>>> GetOrdersByUserIdAsync(Guid userId);
    Task<Result> CreateOrder(Order order);
}