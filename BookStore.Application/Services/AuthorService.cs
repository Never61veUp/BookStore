using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Repositories;

namespace BookStore.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<List<Author>> GetAllAuthorsAsync()
    {
        return await _authorRepository.GetAllAuthorsAsync();
    }
    public async Task<bool> AddAuthorAsync(Author author)
    {
        return await _authorRepository.AddAuthorAsync(author);
    }
    public async Task<bool> UpdateAuthorAsync(Author author)
    {
        return await _authorRepository.UpdateAuthorAsync(author);
    }
    public async Task<bool> DeleteAuthorAsync(Author author)
    {
        return await _authorRepository.DeleteAuthorAsync(author);
    }
    public async Task<Author> GetAuthorByIdAsync(Guid authorId)
    {
        return await _authorRepository.GetAuthorByIdAsync(authorId);
    }
}

public interface IAuthorService
{
    Task<List<Author>> GetAllAuthorsAsync();
    Task<bool> AddAuthorAsync(Author author);
    Task<bool> UpdateAuthorAsync(Author author);
    Task<bool> DeleteAuthorAsync(Author author);
    Task<Author> GetAuthorByIdAsync(Guid authorId);
}