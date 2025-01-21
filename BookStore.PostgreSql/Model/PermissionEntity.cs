using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class PermissionEntity : Entity<int>
{
    public PermissionEntity(int id) : base(id)
    {
    }
    public string Name { get; set; } = string.Empty;
    public ICollection<RoleEntity> Roles { get; set; } = [];
}