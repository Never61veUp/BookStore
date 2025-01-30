using BookStore.Core.Model.Catalog;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Cart;

public enum OrderStatus
{
    Finished,
    Pending,
    Canceled,
}
public class Order
{
    private Order(Guid userId, Dictionary<Book, int> books, OrderStatus orderStatus, DateTime orderDate)
    {
        UserId = userId;
        Books = books;
        OrderStatus = orderStatus;
        OrderDate = orderDate;
    }
    public static Result<Order> Create(Guid userId, Dictionary<Book, int> books, OrderStatus orderStatus, DateTime orderDate)
    {
        return new Order(userId, books, orderStatus, orderDate);
    }
    public static Result<Order> Create(Guid userId, Dictionary<Book, int> books, OrderStatus orderStatus, Cart cart)
    {
        var order = new Order(userId, new Dictionary<Book, int>(books), orderStatus, DateTime.Now);
        cart.Clear();
        return order;
    }
    
    public Guid UserId { get; private set; }
    public Dictionary<Book, int> Books { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public DateTime OrderDate { get; private set; }
    
}