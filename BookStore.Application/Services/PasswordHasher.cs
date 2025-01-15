namespace BookStore.Application.Services;

public interface IPasswordHasher
{
    string GenerateHash(string password);
    bool VerifyHash(string password, string hash);
}

public class PasswordHasher : IPasswordHasher
{
    public string GenerateHash(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);
    
    public bool VerifyHash(string password, string hash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hash);
}