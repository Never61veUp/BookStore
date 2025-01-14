using BookStore.Core.Model.Catalog;

namespace BookStore.PostgreSql.Repositories;

public interface ICategoriesRepository
{
    Task<List<Category>> GetCategoriesAsync();
    Task<Category> GetCategoryByIdAsync(Guid id);
    Task<bool> AddCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(Guid id);
}