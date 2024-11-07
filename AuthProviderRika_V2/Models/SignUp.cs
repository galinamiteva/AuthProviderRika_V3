using System.ComponentModel.DataAnnotations;

namespace AuthProviderRika_V2.Models;

public class SignUp
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
}
