using BookStore.Core.Enums;
using BookStore.PostgreSql.Repositories;
using CSharpFunctionalExtensions;

namespace BookStore.Auth.Services;

public class PermissionService : IPermissionService
{
    private readonly IUserRepository _userRepository;

    public PermissionService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<HashSet<Permission>>> GetPermissionsAsync(Guid userId)
    {
        return await _userRepository.GetUserPermissions(userId);
    }
}

public interface IPermissionService
{
    Task<Result<HashSet<Permission>>> GetPermissionsAsync(Guid userId);
}