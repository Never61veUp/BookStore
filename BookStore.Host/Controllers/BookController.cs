using System.Net.Mime;
using BookStore.Application.Abstractions;
using BookStore.Application.Services;
using BookStore.Auth.Services;
using BookStore.Core.Enums;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
using BookStore.Host.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IImageService _imageService;

    public BookController(IBookService bookService, IImageService imageService)
    {
        _bookService = bookService;
        _imageService = imageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _bookService.GetAllBooksAsync();
        if(books.Count <= 0) return NotFound("Книги не найдены");
        
        return Ok(books);
    }
    [HttpGet("{bookId:guid}")]
    public async Task<IActionResult> GetBookByIdAsync(Guid bookId)
    {
        var book = await _bookService.GetBookByIdAsync(bookId);
        if(book.IsFailure)
            return NotFound(book);
        return Ok(book.Value);
    }
    
    [HttpGet("category/{bookId:guid}")]
    public async Task<IActionResult> GetBookByCategoryIdAsync(Guid bookId)
    {
        var book = await _bookService.GetBooksByCategory(bookId);
        return Ok(book);
    }

    [HttpPost]
    public async Task<IActionResult> AddBookAsync([FromForm] BookRequest bookRequest)
    {
        var price = Price.Create(bookRequest.Price);
        if(price.IsFailure)
            return BadRequest(price.Error);
        
        var book = Book.Create(Guid.NewGuid(), bookRequest.Title, bookRequest.Description, 
            price.Value, bookRequest.AuthorId, bookRequest.CategoryId, bookRequest.StockCount);
        if(book.IsFailure)
            return BadRequest(book.Error);

        if (bookRequest.Image != null)
        {
            var imageResult =  Image.CreateImage(bookRequest.Image.FileName, book.Value.Id);
            if(imageResult.IsFailure)
                return BadRequest(imageResult.Error);
        
            book.Value.AddImage(imageResult.Value);
            await _imageService.UploadImageAsync(bookRequest.Image, "11");
        }
        var addBookTask = await _bookService.AddBookAsync(book.Value);
        
        if(addBookTask.IsFailure)
            return BadRequest(addBookTask.Error);
        
        return Ok(book.Value);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBook(Guid id, [FromForm] BookRequest bookRequest)
    {
        
        var price = Price.Create(bookRequest.Price);
        if(price.IsFailure)
            return BadRequest(price.Error);
        
        var book = Book.Create(id, bookRequest.Title, bookRequest.Description, 
            price.Value, bookRequest.AuthorId, bookRequest.CategoryId, bookRequest.StockCount);
        if(book.IsFailure)
            return BadRequest(book.Error);
        
        if (bookRequest.Image != null)
        {
            var imageResult =  Image.CreateImage(bookRequest.Image.FileName, book.Value.Id);
            if(imageResult.IsFailure)
                return BadRequest(imageResult.Error);
        
            book.Value.AddImage(imageResult.Value);
            await _imageService.UploadImageAsync(bookRequest.Image, "11");
        }
        var updateBookTask = await _bookService.UpdateBookAsync(id, book.Value);
        if(!updateBookTask)
            return BadRequest("Something went wrong");
        
        return Ok(book.Value);
    }
    
    [HttpDelete("{bookId:guid}")]
    public async Task<IActionResult> DeleteBook(Guid bookId)
    {
        var updateBookTask = await _bookService.DeleteBookAsync(bookId);
        return Ok(updateBookTask);
    }
}