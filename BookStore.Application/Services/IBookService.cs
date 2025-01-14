using BookStore.Core.Model.Catalog;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Services;

public interface IBookService
{
    Task<List<Book>> GetAllBooksAsync();
    Task<Book> GetBookByIdAsync(Guid id);
    Task<List<Book>> GetBooksByCategory(Guid categoryId);
    Task<Result> AddBookAsync(Book book);
    Task<bool> UpdateBookAsync(Guid id, Book book);
    Task<bool> DeleteBookAsync(Guid id);

}