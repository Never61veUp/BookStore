using BookStore.Application.Abstractions;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Repositories;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ICategoriesRepository _categoriesRepository;
    private readonly IYandexStorageService _yandexStorageService;
    private readonly IImageService _imageService;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, ICategoriesRepository categoriesRepository, IYandexStorageService yandexStorageService)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _categoriesRepository = categoriesRepository;
        _yandexStorageService = yandexStorageService;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        var books = await _bookRepository.GetAllBooksAsync();
        var tasks = books
            .Where(book => book.Image != null)
            .Select(async book =>
            {
                var url = await _yandexStorageService.GetPreSignedUrlAsync(book.Image.Name, TimeSpan.FromMinutes(15));
                book.Image.SetImageLink(url);
            });

        await Task.WhenAll(tasks);
        return books;
    }

    public async Task<List<Book>> GetBooksByCategory(Guid categoryId)
    {
        return await _bookRepository.GetBooksByCategoryAsync(categoryId);
    }
    public async Task<Result<Book>> GetBookByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        if(book.IsFailure)
            return Result.Failure<Book>(book.Error);
        
        var url = await _yandexStorageService.GetPreSignedUrlAsync(book.Value.Image.Name, TimeSpan.FromMinutes(15));
        book.Value.Image.SetImageLink(url);
        return Result.Success<Book>(book.Value);
    }
    public async Task<Result> AddBookAsync(Book book)
    {
        return await _bookRepository.AddBookAsync(book);
    }
    public async Task<bool> UpdateBookAsync(Guid id, Book book)
    {
        return await _bookRepository.UpdateBookAsync(id, book);
    }
    public async Task<bool> DeleteBookAsync(Guid id)
    {
        return await _bookRepository.DeleteBookAsync(id);
    }
}