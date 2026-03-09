using System.ComponentModel.DataAnnotations;

namespace InstaClone.Api.DTOs;

public class ProfileResponse
{
    public string Username { get; set; } = default!;
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public int PostCount { get; set; }
}

public class UpdateProfileRequest
{
    [StringLength(100)]
    public string? DisplayName { get; set; }

    [StringLength(500)]
    public string? Bio { get; set; }

    public string? AvatarUrl { get; set; }
}
