using BookStore.Application.Abstractions;
using BookStore.Host.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private const string COOKIE_KEY = "tasty-cookies";
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("signUp")]
    public async Task<IActionResult> SignUp([FromBody] SignUpUserRequest signUpUserRequest)
    {
        var signUpResult = await _userService.SignUpAsync(
            signUpUserRequest.FirstName,
            signUpUserRequest.LastName,
            signUpUserRequest.Email,
            signUpUserRequest.Password,
            signUpUserRequest.MiddleName);

        if (signUpResult.IsFailure)
            return BadRequest(signUpResult.Error);

        return Ok(signUpResult);
    }

    [HttpPost("signIn")]
    public async Task<IActionResult> SignIn(LoginUserRequest loginUserRequest)
    {
        var token = await _userService.SignInAsync(
            loginUserRequest.Email, loginUserRequest.Password);

        if (token.IsFailure)
            return BadRequest(token.Error);

        HttpContext.Response.Cookies.Append(COOKIE_KEY, token.Value, new CookieOptions
        {
            HttpOnly = true,
            Secure = true
        });

        return Ok(token.Value);
    }

    [HttpPost("signOut")]
    public new IActionResult SignOut()
    {
        HttpContext.Response.Cookies.Delete(COOKIE_KEY);
        return Ok(new { success = true, message = "Вы успешно вышли из системы" });
    }

    [HttpGet("isLoggedIn")]
    public IActionResult IsLoggedIn()
    {
        return Ok(HttpContext.Request.Cookies.ContainsKey(COOKIE_KEY));
    }
}