using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class OrderItemEntity : Entity<Guid>
{
    public OrderItemEntity(Guid id) : base(id)
    {
        
    }
    public Guid OrderId { get; set; }
    public OrderEntity Order { get; set; }
    public Guid BookId { get; set; }
    [Required]
    public int Quantity { get; set; }
}