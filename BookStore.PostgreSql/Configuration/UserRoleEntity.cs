﻿namespace BookStore.PostgreSql.Configuration;

public class UserRoleEntity
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
}