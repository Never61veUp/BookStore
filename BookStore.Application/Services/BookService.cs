using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Repositories;

namespace BookStore.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllBooksAsync();
    }
    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        return await _bookRepository.GetBookByIdAsync(id);
    }
    public async Task<bool> AddBookAsync(Book book)
    {
        return await _bookRepository.AddBookAsync(book);
    }
    public async Task<bool> UpdateBookAsync(Book book)
    {
        return await _bookRepository.UpdateBookAsync(book);
    }
    public async Task<bool> DeleteBookAsync(Guid id)
    {
        return await _bookRepository.DeleteBookAsync(id);
    }
}