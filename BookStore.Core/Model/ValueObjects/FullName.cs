using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.ValueObjects;

public class FullName : ValueObject
{
    private FullName(string firstName, string lastName, string? middleName = null)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }
    
    public string FirstName { get; }
    public string LastName { get; }
    public string? MiddleName { get; }
    
    public static Result<FullName> Create(string firstName, string lastName, string? middleName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<FullName>("First name cannot be empty");

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<FullName>("Last name cannot be empty");

        return new FullName(firstName, lastName, middleName);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        if (MiddleName != null) yield return MiddleName;
    }
    public override string ToString()
    {
        return $"{LastName} {FirstName[0]} {MiddleName?[0] ?? ' '}";
    }
}