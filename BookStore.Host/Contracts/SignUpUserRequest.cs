using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Host.Contracts;

public record SignUpUserRequest([Required]string FirstName, 
    [Required]string LastName, [EmailAddress]string Email, 
    [Required]string Password, string MiddleName = "");