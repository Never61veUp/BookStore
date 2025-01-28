using BookStore.Application.Abstractions;
using BookStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly ICartService _cartService;

    public CartController(ICartService cartService, IBookService bookService)
    {
        _cartService = cartService;
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        if (!TryGetUserId(out var id))
            return Unauthorized();

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);

        return Ok(cart.Value.Books);
    }

    [HttpGet("total-price")]
    public async Task<IActionResult> GetTotalPrice()
    {
        if (!TryGetUserId(out var id))
            return Unauthorized();

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return NotFound(cart.Error);

        return Ok(cart.Value.GetTotalPrice().Value);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddBookToCart(Guid bookId)
    {
        if (!TryGetUserId(out var id))
            return Unauthorized();

        var addBookResult = await _cartService.AddBookToCartAsync(id, bookId);
        if (addBookResult.IsFailure)
            return BadRequest(addBookResult.Error);

        return Ok("Книга успешно добавлена в корзину.");
    }

    [HttpPatch("items/{bookId:guid}/increase")]
    public async Task<IActionResult> IncreaseQuantity(Guid bookId)
    {
        if (!TryGetUserId(out var id))
            return Unauthorized();

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);

        var book = await _bookService.GetBookByIdAsync(bookId);
        if (book.IsFailure)
            return BadRequest(book.Error);

        var addBook = cart.Value.AddBook(book.Value);
        if (addBook.IsFailure)
            return BadRequest(addBook.Error);

        await _cartService.UpdateCart(cart.Value);

        return Ok("Количество увеличено.");
    }

    [HttpPatch("items/{bookId:guid}/decreaseQuantityByOne")]
    public async Task<IActionResult> DecreaseQuantityByOne(Guid bookId)
    {
        if (!TryGetUserId(out var id))
            return Unauthorized();

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);

        var book = await _bookService.GetBookByIdAsync(bookId);
        if (book.IsFailure)
            return BadRequest(book.Error);

        var removeBook = cart.Value.RemoveBook(book.Value);
        if (removeBook.IsFailure)
            return BadRequest(removeBook.Error);

        await _cartService.UpdateCart(cart.Value);

        return Ok();
    }

    [HttpDelete("items/{bookId:guid}")]
    public async Task<IActionResult> RemoveBookFromCart(Guid bookId)
    {
        if (!TryGetUserId(out var id))
            return Unauthorized();

        var cart = await _cartService.GetCartAsync(id);
        if (cart.IsFailure)
            return BadRequest(cart.Error);

        var book = await _bookService.GetBookByIdAsync(bookId);
        if (book.IsFailure)
            return BadRequest(book.Error);

        cart.Value.AddBook(book.Value);
        await _cartService.UpdateCart(cart.Value);

        return Ok("Книга удалена из корзины.");
    }

    private bool TryGetUserId(out Guid id)
    {
        id = Guid.Empty;
        var userId = User.FindFirst("userId")?.Value;
        if (userId is null || !Guid.TryParse(userId, out id))
            return false;

        return true;
    }
}