﻿using BookStore.Application.Abstractions;
using BookStore.Application.Services;
using BookStore.Core.Model.Cart;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IBookService _bookService;
    private readonly AuthorizationHandlerContext _context;

    public CartController(ICartService cartService, IBookService bookService)
    {
        _cartService = cartService;
        _bookService = bookService;
    }

    [HttpGet("getCard")]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized("Не авторизован");
        }

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);
        return Ok(cart.Value.Books);
    }

    [HttpGet("getTotalPrice")]
    public async Task<IActionResult> GetTotalPrice()
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized("Не авторизован");
        }

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);
        return Ok(cart.Value.GetTotalPrice().Value);
    }

    [HttpPost("addToCard")]
    public async Task<IActionResult> AddBookToCart(Guid bookId)
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized("Не авторизирован");
        }

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);
        
        var addBookResult = await _cartService.AddBookToCartAsync(id, bookId);
        if (addBookResult.IsFailure)
            return BadRequest(addBookResult.Error);

        return Ok("Успешно");
    }

    [HttpPost("increaseQuantity")]
    public async Task<IActionResult> IncreaseQuantity(Guid bookId)
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized("Не авторизирован");
        
        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);
        
        var book = await _bookService.GetBookByIdAsync(bookId);
        if(book.IsFailure)
            return BadRequest(book.Error);
        
        var addBook = cart.Value.AddBook(book.Value);
        if (addBook.IsFailure)
            return BadRequest(addBook.Error);
        
        await _cartService.UpdateCart(cart.Value);
        
        return Ok();
    }
    [HttpPost("decreaseQuantityByOne")]
    public async Task<IActionResult> decreaseQuantityByOne(Guid bookId)
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized("Не авторизирован");
        
        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);
        
        var book = await _bookService.GetBookByIdAsync(bookId);
        if(book.IsFailure)
            return BadRequest(book.Error);
        
        var removeBook = cart.Value.RemoveBook(book.Value);
        if(removeBook.IsFailure)
            return BadRequest(removeBook.Error);
        
        await _cartService.UpdateCart(cart.Value);
        
        return Ok();
    }

    [HttpPost("removeFromCart")]
    public async Task<IActionResult> RemoveBookFromCart(Guid bookId)
    {
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out var id))
            return Unauthorized("Не авторизирован");

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);
        var book = await _bookService.GetBookByIdAsync(bookId);
        cart.Value.AddBook(book.Value);
        await _cartService.UpdateCart(cart.Value);
        
        return Ok("Успешно");
    }
}