using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class AuthorEntity : Entity<Guid>
{
    public FullName FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Biography { get; set; }
}