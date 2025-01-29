using System.Collections.ObjectModel;
using BookStore.Core.Model.Cart;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class OrderEntity : Entity<Guid>
{
    public Guid UserId { get; set; }
    public ICollection<OrderItemEntity> OrderItems { get; set; }
    public OrderStatus OrderStatus { get;set; }
    public DateTime OrderDate { get;set; }
}