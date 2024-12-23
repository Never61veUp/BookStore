using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;

namespace BookStore.PostgreSql.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public AuthorRepository(BookStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        var authorEntities = await _dbContext.Authors.AsNoTracking().ToListAsync();
        
        return _mapper.Map<List<Author>>(authorEntities);
    }
    public async Task<bool> AddAuthorAsync(Author author)
    {
        var authorEntity = _mapper.Map<AuthorEntity>(author);
        await _dbContext.Authors.AddAsync(authorEntity);
        
        return await _dbContext.SaveChangesAsync() != 0;
    }
    public async Task<bool> UpdateAuthorAsync(Author author)
    {
        var authorEntity = _mapper.Map<AuthorEntity>(author);
        _dbContext.Update(authorEntity);
        
        return await _dbContext.SaveChangesAsync() != 0;
    }
    public async Task<bool> DeleteAuthorAsync(Author author)
    {
        var authorEntity = _mapper.Map<AuthorEntity>(author);
        _dbContext.Remove(authorEntity);
        
        return await _dbContext.SaveChangesAsync() != 0;
    }
    public async Task<Author> GetAuthorByIdAsync(Guid authorId)
    {
        var authorEntity = await _dbContext.Authors.FindAsync(authorId);
        return _mapper.Map<Author>(authorEntity);
    }
}

public interface IAuthorRepository
{
    Task<List<Author>> GetAllAuthorsAsync();
    Task<bool> AddAuthorAsync(Author author);
    Task<bool> UpdateAuthorAsync(Author author);
    Task<bool> DeleteAuthorAsync(Author author);
    Task<Author> GetAuthorByIdAsync(Guid authorId);
}