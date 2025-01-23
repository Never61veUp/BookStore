﻿using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;

namespace BookStore.PostgreSql.Model;

public class CartItemEntity : Entity<Guid>
{
    [Required]
    public Guid CartId { get; set; }
    public CartEntity Cart { get; set; }
    [Required]
    public Guid BookId { get; set; }
    [Required]
    public int Quantity { get; set; }
}