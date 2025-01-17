using BookStore.Application.Services;
using BookStore.Host.Contracts;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Host.Controllers;
[ApiController]
[Route("[controller]/api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> SignUp([FromBody]SignUpUserRequest signUpUserRequest)
    {
        var signUpResult = await _userService.SignUpAsync(
            signUpUserRequest.FirstName, 
            signUpUserRequest.LastName, 
            signUpUserRequest.Email, 
            signUpUserRequest.Password, 
            signUpUserRequest.MiddleName);
        
        if(signUpResult.IsFailure)
            return BadRequest(signUpResult.Error);
        
        return Ok(signUpResult);
    }
    
    [HttpPost("signIn")]
    public async Task<IActionResult> SignIn(LoginUserRequest loginUserRequest)
    {
        var token = await _userService.SignInAsync(
            loginUserRequest.Email, loginUserRequest.Password);
        
        if(token.IsFailure)
            return BadRequest(token.Error);
        
        HttpContext.Response.Cookies.Append("tasty-cookies", token.Value);
        
        return Ok(token.Value);
    }
}