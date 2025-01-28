using System.ComponentModel.DataAnnotations;

namespace BookStore.Host.Contracts;

public record SignInUserRequest(
    [Required]string Email, 
    [Required]string Password);