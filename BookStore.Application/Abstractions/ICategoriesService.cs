using BookStore.Core.Model.Catalog;

namespace BookStore.Application.Abstractions;

public interface ICategoriesService
{
    Task<List<Category>> GetCategories();
    Task<Category> GetCategoryById(Guid id);
    Task<bool> CreateCategory(Category category);
    Task<bool> UpdateCategory(Category category);
    Task<bool> DeleteCategory(Guid id);
}