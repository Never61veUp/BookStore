namespace BookStore.Host.Contracts;

public record AuthorRequest(string FirstName, string LastName, DateTime DateOfBirth, string Biography, string MiddleName = "");