using CSharpFunctionalExtensions;

namespace BookStore.Application.Abstractions;

public interface IUserService
{
    Task<Result> SignUpAsync(string firstName, string lastName, string email, string password, string? middleName = "");
    Task<Result<string>> SignInAsync(string email, string password);
}