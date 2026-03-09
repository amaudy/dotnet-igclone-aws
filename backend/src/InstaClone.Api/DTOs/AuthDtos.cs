using System.ComponentModel.DataAnnotations;

namespace InstaClone.Api.DTOs;

public class RegisterRequest
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string Username { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = default!;
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}

public class TokenResponse
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
