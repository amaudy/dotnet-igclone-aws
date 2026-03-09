using Microsoft.AspNetCore.Identity;

namespace InstaClone.Api.Models;

public class AppUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
}
