namespace BookStore.Host.Contracts;

public record SignUpUserRequest(string FirstName, string LastName, string Email, string Password, string MiddleName = "");