using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.PostgreSql.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public BookRepository(BookStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<Book>> GetAllBooksAsync()
    {
        var bookEntities = await _dbContext.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();
        
        var books = _mapper.Map<List<Book>>(bookEntities);
        return books;
    }

    public async Task<List<Book>> GetBooksByCategoryAsync(Guid categoryId)
    {
        var bookEntities = await _dbContext.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .Where(b => b.CategoryId == categoryId)
            .ToListAsync();
        
        var books = _mapper.Map<List<Book>>(bookEntities);
        return books;
    }
    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        var bookEntity = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<Book>(bookEntity);
    }
    public async Task<Result> AddBookAsync(Book book)
    {
        var authorEntity = await _dbContext.Authors.FirstOrDefaultAsync(x => x.Id == book.AuthorId);
        if (authorEntity == null)
            return Result.Failure("Author entity not found");

        var categoryEntity = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == book.CategoryId);
        if (categoryEntity == null)
            return Result.Failure("Category entity not found");

        var bookEntity = new BookEntity(book.Id)
        {
            Title = book.Title,
            Description = book.Description,
            Price = book.Price,
            Author = authorEntity,
            Category = categoryEntity,
            StockCount = book.StockCount
        };
        await _dbContext.Books.AddAsync(bookEntity);
        return await _dbContext.SaveChangesAsync() > 0 
            ? Result.Success()
            : Result.Failure("Failed to add book");
    }
    public async Task<bool> UpdateBookAsync(Book book)
    {
        var bookEntity = _mapper.Map<BookEntity>(book);
        _dbContext.Update(bookEntity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
    public async Task<bool> DeleteBookAsync(Guid id)
    {
        var bookEntity = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
        if (bookEntity != null) 
            _dbContext.Books.Remove(bookEntity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}