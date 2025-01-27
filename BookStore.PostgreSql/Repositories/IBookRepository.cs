using BookStore.Core.Model.Catalog;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Repositories;

public interface IBookRepository
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Result<Book>> GetBookByIdAsync(Guid id);
    Task<List<Book>> GetBooksByCategoryAsync(Guid categoryId);
    Task<Result> AddBookAsync(Book book);
    Task<bool> UpdateBookAsync(Guid id, Book book);
    Task<bool> DeleteBookAsync(Guid id);

}