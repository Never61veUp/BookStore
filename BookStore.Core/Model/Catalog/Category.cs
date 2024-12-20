using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Catalog;

public class Category : Entity<Guid>
{
    private Category(Guid id, string name, Guid? parentCategoryId) : base(id)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
    }
    
    public string Name { get; private set; }
    public Guid? ParentCategoryId { get; private set; }

    public static Result<Category> Create(Guid id, string name, Guid? parentCategoryId)
    {
        if(string.IsNullOrWhiteSpace(name))
            return Result.Failure<Category>("Name cannot be empty");
                
        return Result.Success(new Category(id, name, parentCategoryId));
    }
}