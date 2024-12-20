using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class CategoryEntity : Entity<Guid>
{
    public string Name { get; set; }
    public CategoryEntity ParentCategory { get; set; }
}