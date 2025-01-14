using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Repositories;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICategoriesRepository _categoriesRepository;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, ICategoriesRepository categoriesRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _categoriesRepository = categoriesRepository;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllBooksAsync();
        return books;
    }

    public async Task<List<Book>> GetBooksByCategory(Guid categoryId)
    {
        return await _bookRepository.GetBooksByCategoryAsync(categoryId);
    }
    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        return await _bookRepository.GetBookByIdAsync(id);
    }
    public async Task<Result> AddBookAsync(Book book)
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