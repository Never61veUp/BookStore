using System.Net.Mime;
using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;
[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IImageService _imageService;
    private readonly IYandexStorageService _yandexStorageService;

    public BookController(IBookService bookService, IImageService imageService, IYandexStorageService yandexStorageService)
    {
        _bookService = bookService;
        _imageService = imageService;
        _yandexStorageService = yandexStorageService;
    }

    [HttpGet("GetBooks")]
    public async Task<IActionResult> GetBooks()
    {
        return Ok(await _bookService.GetAllBooksAsync());
    }
    [HttpGet("GetBookById")]
    public async Task<IActionResult> GetBookByIdAsync(Guid bookId)
    {
        var book = await _bookService.GetBookByIdAsync(bookId);
        return Ok(book);
    }
    [HttpGet("GetBookByCategoryId")]
    public async Task<IActionResult> GetBookByCategoryIdAsync(Guid bookId)
    {
        var book = await _bookService.GetBooksByCategory(bookId);
        return Ok(book);
    }

    [HttpPost("AddBook")]
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
        }

        var addBookTask = await _bookService.AddBookAsync(book.Value);
        await _imageService.UploadImageAsync(bookRequest.Image, "11");
        if(addBookTask.IsFailure)
            return BadRequest(addBookTask.Error);
        return Ok(book.Value);
    }
    [HttpPut("UpdateBook")]
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
        await _bookService.UpdateBookAsync(id, book.Value);
        return Ok(book.Value);
    }
    [HttpDelete("DeleteBook")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var updateBookTask = await _bookService.DeleteBookAsync(id);
        return Ok(updateBookTask);
    }
}
public record BookRequest(string Title, string Description, decimal Price, Guid AuthorId, Guid CategoryId, int StockCount, IFormFile? Image);