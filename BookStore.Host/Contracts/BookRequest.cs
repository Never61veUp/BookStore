namespace BookStore.Host.Contracts;

public record BookRequest(string Title, string Description, 
    decimal Price, Guid AuthorId, Guid CategoryId, 
    int StockCount, IFormFile? Image);