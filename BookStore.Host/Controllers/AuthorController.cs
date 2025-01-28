using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
using BookStore.Host.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAuthors()
    {
        var result = await _authorService.GetAllAuthorsAsync();
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddAuthor(AuthorRequest request)
    {
        var fullName = FullName.Create(request.FirstName, request.LastName, request.MiddleName);
        if(fullName.IsFailure)
            return BadRequest(fullName.Error);
        
        var author = Author.Create(Guid.NewGuid(), fullName.Value, request.DateOfBirth, request.Biography);
        if(author.IsFailure)
            return BadRequest(author.Error);
        
        var addAuthorTask = await _authorService.AddAuthorAsync(author.Value);
        
        if(!addAuthorTask)
            return BadRequest("Something went wrong");
        
        return Ok();
    }
}