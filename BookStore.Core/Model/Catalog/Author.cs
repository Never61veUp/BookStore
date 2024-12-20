using BookStore.Core.Model.ValueObjects;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.Catalog;

public class Author : Entity<Guid>
{
    public Author(Guid id, FullName fullName, DateTime birthDate, string biography) : base(id)
    {
        FullName = fullName;
        BirthDate = birthDate;
        Biography = biography;
    }
    
    public FullName FullName { get; private set; }
    public DateTime? BirthDate { get; private set; }
    public string? Biography { get; private set; }

    public static Result<Author> Create(Guid id, FullName fullName, DateTime birthDate, string biography)
    {
        if(birthDate > DateTime.Today)
            return Result.Failure<Author>("Birth date cannot be in the future.");
        if(string.IsNullOrWhiteSpace(biography))
            return Result.Failure<Author>("Biography cannot be empty.");
        
        return Result.Success(new Author(id, fullName, birthDate, biography));
    }
}