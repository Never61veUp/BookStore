using BookStore.Core.Model.Users;
using BookStore.PostgreSql.Model;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.PostgreSql.Repositories;

public interface IUserRepository
{
    Task<Result> AddUserAsync(User user);
    Task<Result<User>> GetUserByEmail(string email);
}

public class UserRepository : IUserRepository
{
    private readonly BookStoreDbContext _dbContext;

    public UserRepository(BookStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result> AddUserAsync(User user)
    {
        var userEntity = new UserEntity(user.Id)
        {
            Name = user.Name,
            Email = user.Email,
            PasswordHash = user.PasswordHash
        };
        await _dbContext.Users.AddAsync(userEntity);
        return await _dbContext.SaveChangesAsync() != 0
            ? Result.Success()
            : Result.Failure("Something went wrong");
    }

    public async Task<Result<User>> GetUserByEmail(string email)
    {
        var userEntity = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (userEntity == null)
            return Result.Failure<User>("User not found");
        var user = User.Create(
            userEntity.Id,
            userEntity.Name,
            userEntity.Email,
            userEntity.PasswordHash
        );
        if(user.IsFailure)
            return Result.Failure<User>(user.Error);
        return Result.Success<User>(user.Value);
    }
}