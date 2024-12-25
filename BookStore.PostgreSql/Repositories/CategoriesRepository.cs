using AutoMapper;
using BookStore.Core.Model.Catalog;
using BookStore.PostgreSql.Model;
using Microsoft.EntityFrameworkCore;

namespace BookStore.PostgreSql.Repositories;

public class CategoriesRepository : ICategoriesRepository
{
    private readonly BookStoreDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoriesRepository(BookStoreDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<Category>> GetCategoriesAsync()
    {
        var categoriesEntity = await _dbContext.Categories.ToListAsync();
        return _mapper.Map<List<Category>>(categoriesEntity);
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id)
    {
        var categoryEntity = await _dbContext.Categories.FindAsync(id);
        return _mapper.Map<Category>(categoryEntity);
    }
    public async Task<bool> AddCategoryAsync(Category category)
    {
        var categoryEntity = _mapper.Map<CategoryEntity>(category);
        await _dbContext.Categories.AddAsync(categoryEntity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        var categoryEntity = _mapper.Map<CategoryEntity>(category);
        _dbContext.Categories.Update(categoryEntity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var categoryEntity = await _dbContext.Categories.FindAsync(id);
        if (categoryEntity != null) 
            _dbContext.Categories.Remove(categoryEntity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}