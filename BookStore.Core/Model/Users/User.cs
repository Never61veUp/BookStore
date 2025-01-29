using System.Collections.ObjectModel;
using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Users;

public class User : Entity<Guid>, IAggregateRoot
{
    public FullName Name { get; }
    public string Email { get; }
    public string PasswordHash { get; }

    private User(Guid id, FullName name, string email, string passwordHash) : base(id)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }

    public static Result<User> Create(Guid id, FullName name, string email, string passwordHash)
    {
        return new User(id, name, email, passwordHash);
    }

    public static Result<User> Create(Guid id, FullName name, string email, string passwordHash,
        string[] emails)
    {
        if (emails.Contains(email))
            return Result.Failure<User>("Email is already registered");
        
        return new User(id, name, email, passwordHash);
    }
}