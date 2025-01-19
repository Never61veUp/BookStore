using BookStore.Application.Abstractions;
using BookStore.Auth.Abstractions;
using BookStore.Auth.Services;
using BookStore.Core.Model.Users;
using BookStore.Core.Model.ValueObjects;
using BookStore.PostgreSql.Repositories;
using CSharpFunctionalExtensions;

namespace BookStore.Application.Services;

public class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IJwtProvider _jwtProvider;

    public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository, IJwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result> SignUpAsync(string firstName, string lastName, 
        string email, string password, string? middleName = "")
    {
        var fullName = FullName.Create(firstName, lastName, middleName);
        if(fullName.IsFailure)
            return Result.Failure(fullName.Error);
        
        var hashedPassword = _passwordHasher.GenerateHash(password);
        
        var user =  User.Create(Guid.NewGuid(), fullName.Value, email, hashedPassword);
        if(user.IsFailure)
            return Result.Failure(user.Error);
        
        return await _userRepository.AddUserAsync(user.Value);
    }

    public async Task<Result<string>> SignInAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        //TODO: validation
        if(user.IsFailure)
            return Result.Failure<string>(user.Error);

        var result = _passwordHasher.VerifyHash(password, user.Value.PasswordHash);
        if(!result)
            return Result.Failure<string>("Password or Email is invalid");
        var token = _jwtProvider.GenerateToken(user.Value);
        return Result.Success(token);
        
    }
}