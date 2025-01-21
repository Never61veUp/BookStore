using BookStore.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Auth.Services;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(Permission[] permissions)
    {
        Permissions = permissions;
    }
    public Permission[] Permissions { get; set; } = [];
}