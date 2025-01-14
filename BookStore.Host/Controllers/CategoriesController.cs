using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;
[ApiController]
[Route("[controller]")]
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
        return Ok(await _categoriesService.GetCategories());
    }
    [HttpGet("GetBookById/{id}")]
    public async Task<IActionResult> GetCategoryById(Guid bookId)
    {
        var book = await _categoriesService.GetCategoryById(bookId);
        return Ok(book);
    }

    [HttpPost("AddBook")]
    public async Task<IActionResult> AddCategory(CategoryRequest categoryRequest)
    {
        var category = Category.Create(Guid.NewGuid(), categoryRequest.Title, categoryRequest.ParentId);
        if(category.IsFailure)
            return BadRequest(category);
        await _categoriesService.CreateCategory(category.Value);
        return Ok(category.Value);
    }
}
public record CategoryRequest(string Title, Guid? ParentId);