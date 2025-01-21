using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public sealed class UserEntity : Entity<Guid>
{
    public UserEntity(Guid id) : base(id)
    {
    }
    
    public FullName Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<RoleEntity> Roles { get; set; }
}