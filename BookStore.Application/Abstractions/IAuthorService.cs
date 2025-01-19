using BookStore.Core.Model.Catalog;

namespace BookStore.Application.Services;

public interface IAuthorService
{
    Task<List<Author>> GetAllAuthorsAsync();
    Task<bool> AddAuthorAsync(Author author);
    Task<bool> UpdateAuthorAsync(Author author);
    Task<bool> DeleteAuthorAsync(Author author);
    Task<Author> GetAuthorByIdAsync(Guid authorId);
}