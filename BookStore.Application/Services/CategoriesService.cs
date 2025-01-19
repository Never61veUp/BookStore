using BookStore.Application.Abstractions;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Repositories;

namespace BookStore.Application.Services;

public class CategoriesService : ICategoriesService
{
    private readonly ICategoriesRepository _categoriesRepository;

    public CategoriesService(ICategoriesRepository categoriesRepository)
    {
        _categoriesRepository = categoriesRepository;
    }

    public async Task<List<Category>> GetCategories()
    {
        return await _categoriesRepository.GetCategoriesAsync();
    }

    public async Task<Category> GetCategoryById(Guid id)
    {
        return await _categoriesRepository.GetCategoryByIdAsync(id);
    }

    public async Task<bool> CreateCategory(Category category)
    {
        return await _categoriesRepository.AddCategoryAsync(category);
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        return await _categoriesRepository.UpdateCategoryAsync(category);
    }

    public async Task<bool> DeleteCategory(Guid id)
    {
        return await _categoriesRepository.DeleteCategoryAsync(id);
    }
}