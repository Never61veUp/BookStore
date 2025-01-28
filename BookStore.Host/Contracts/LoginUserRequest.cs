using System.ComponentModel.DataAnnotations;

namespace BookStore.Host.Contracts;

public record LoginUserRequest(
    [Required][EmailAddress]string Email, 
    [Required]string Password);