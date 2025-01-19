using BookStore.Core.Model.Users;

namespace BookStore.Auth.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}