using BookStore.Application.Abstractions;
using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Host.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoriesService _categoriesService;

    public CategoriesController(ICategoriesService categoriesService)
    {
        _categoriesService = categoriesService;
    }
    
    [HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoriesService.GetCategories();
        return Ok(categories);
    }
    
    [HttpGet("GetBookById/{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid categoryId)
    {
        var category = await _categoriesService.GetCategoryById(categoryId);
        return Ok(category);
    }

    [HttpPost("AddCategory")]
    public async Task<IActionResult> AddCategory(CategoryRequest categoryRequest)
    {
        var category = Category.Create(
            Guid.NewGuid(), categoryRequest.Title, categoryRequest.ParentId);
        if(category.IsFailure)
            return BadRequest(category);
        
        await _categoriesService.CreateCategory(category.Value);
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Value.Id }, category.Value);
    }
}