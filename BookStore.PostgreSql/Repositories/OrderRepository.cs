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
            .Include(o => o.OrderItems) // Загружаем OrderItems сразу
            .ToListAsync();
        if (orderEntities.Count == 0)
        {
            return Result.Success<IEnumerable<Order>>(new List<Order>());
        }
        
        var orderItemEntities = orderEntities.SelectMany(o => o.OrderItems).ToList();
        var bookIds = orderItemEntities.Select(x => x.BookId).Distinct().ToList();
        var bookEntitiesById = await GetBooksByIdsAsync(bookIds);

        var bookResults = MapCartItemsToBooks(orderItemEntities, bookEntitiesById);

        if (bookResults.Any(r => r.IsFailure))
        {
            var firstError = bookResults.First(r => r.IsFailure).Error;
            return Result.Failure<IEnumerable<Order>>(firstError);
        }

        var bookDictionary = bookResults
            .Select(r => r.Value)
            .GroupBy(x => x.Item1)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.Item2));

        var orders = orderEntities
            .Select(entity => Order.Create(
                entity.UserId,
                bookDictionary,
                entity.OrderStatus,
                entity.OrderDate))
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
        var orderEntity = new OrderEntity()
        {
            UserId = order.UserId,
            OrderStatus = order.OrderStatus,
        };
        var orderItems = order.Books.Select(x => new OrderItemEntity()
        {
            BookId = x.Key.Id,
            Order = orderEntity,
            Quantity = x.Value
        }).ToList();
        orderEntity.OrderItems = orderItems;
        _dbContext.Orders.Add(orderEntity);
        _dbContext.OrderItems.AddRange(orderItems);
        var result = await _dbContext.SaveChangesAsync() > 0
            ? Result.Success()
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