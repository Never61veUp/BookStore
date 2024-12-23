using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }
[HttpGet("GetAuthors")]
    public async Task<IActionResult> GetAuthors()
    {
        var result = await _authorService.GetAllAuthorsAsync();
        return Ok(result);
    }
[HttpPost("AddAuthor")]
    public async Task<IActionResult> AddAuthor(AuthorRequest request)
    {
        var fullName = FullName.Create(request.FirstName, request.LastName, request.MiddleName);
        if(fullName.IsFailure)
            return BadRequest(fullName.Error);
        var author = Author.Create(Guid.NewGuid(), fullName.Value, request.DateOfBirth, request.Biography);
        if(author.IsFailure)
            return BadRequest(author.Error);
        await _authorService.AddAuthorAsync(author.Value);
        return Ok();
    }
}
public record AuthorRequest(string FirstName, string LastName, DateTime DateOfBirth, string Biography, string MiddleName = "");