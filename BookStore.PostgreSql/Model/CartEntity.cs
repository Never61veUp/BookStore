﻿using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class CartEntity(Guid id) : Entity<Guid>(id)
{
    [Required]
    public Guid UserId { get; set; }
    public ICollection<CartItemEntity> Items { get; set; } = new List<CartItemEntity>();
}