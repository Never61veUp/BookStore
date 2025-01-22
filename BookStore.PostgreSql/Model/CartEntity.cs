using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class CartEntity() : Entity<Guid>
{
    public override Guid Id { get; protected set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
}