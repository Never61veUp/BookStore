using BookStore.Core.Model.Catalog;

namespace BookStore.PostgreSql.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(Guid id);
    Task<bool> AddBookAsync(Book book);
    Task<bool> UpdateBookAsync(Book book);
    Task<bool> DeleteBookAsync(Guid id);

}