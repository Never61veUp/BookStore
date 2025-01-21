using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class RoleEntity : Entity<int>
{
    public RoleEntity(int id) : base(id) { }
    
    public string Name {get; set;} = string.Empty;
    public ICollection<PermissionEntity> GlobalPermissions { get; set; } = [];
    public ICollection<UserEntity> Users { get; set; } = [];

}