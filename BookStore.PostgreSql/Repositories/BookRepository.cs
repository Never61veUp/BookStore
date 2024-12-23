using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
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
        var bookEntities = await _dbContext.Books.ToListAsync();
        
        return _mapper.Map<List<Book>>(bookEntities);
    }
    public async Task<Book> GetBookByIdAsync(Guid id)
    {
        var bookEntity = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<Book>(bookEntity);
    }
    public async Task<bool> AddBookAsync(Book book)
    {
        var bookEntity = _mapper.Map<BookEntity>(book);
        await _dbContext.Books.AddAsync(bookEntity);
        return await _dbContext.SaveChangesAsync() > 0;
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