namespace BookStore.Host.Contracts;

public record CategoryRequest(string Title, Guid? ParentId);