using System.Globalization;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Model.ValueObjects;

public class Price : ValueObject
{
    private Price(decimal value)
    {
        Value = value;
    }
    
    public decimal Value { get; }

    public static Result<Price> Create(decimal value)
    {
        if(value < 0)
            return Result.Failure<Price>("Price cannot be negative");
        
        return Result.Success(new Price(value));
    }

    public override string ToString()
    {
        var parts = Value.ToString("F", CultureInfo.InvariantCulture).Split('.');
        var integerPart = parts[0];
        var fractionalPart = parts.Length > 1 ? parts[1] : "";
        
        var formattedIntegerPart = string.Format(CultureInfo.InvariantCulture, "{0:n0}", decimal.Parse(integerPart))
            .Replace(",", "'");
        
        return $"{formattedIntegerPart},{fractionalPart}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}