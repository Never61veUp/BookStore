﻿using BookStore.Application.Services;
using BookStore.Core.Model.Catalog;
using BookStore.Core.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;
[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet("GetBooks")]
    public async Task<IActionResult> GetBooks()
    {
        return Ok(await _bookService.GetAllBooksAsync());
    }
    [HttpGet("GetBookById/{id}")]
    public async Task<IActionResult> GetBookByIdAsync(Guid bookId)
    {
        var book = _bookService.GetBookByIdAsync(bookId);
        return Ok(book);
    }

    [HttpPost("AddBook")]
    public async Task<IActionResult> AddBookAsync(BookRequest bookRequest)
    {
        var price = Price.Create(bookRequest.Price);
        if(price.IsFailure)
            return BadRequest(price.Error);
        
        var book = Book.Create(Guid.NewGuid(), bookRequest.Title, bookRequest.Description, 
            price.Value, bookRequest.AuthorId, bookRequest.CategoryId, bookRequest.StockCount);
        if(book.IsFailure)
            return BadRequest(book.Error);
        await _bookService.AddBookAsync(book.Value);
        return Ok(book);
    }
}
public record BookRequest(string Title, string Description, decimal Price, Guid AuthorId, Guid CategoryId, int StockCount);