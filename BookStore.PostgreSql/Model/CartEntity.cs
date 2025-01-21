namespace BookStore.PostgreSql.Model;

public class CartEntity
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
}