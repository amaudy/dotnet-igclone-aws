using System.ComponentModel.DataAnnotations;

namespace InstaClone.Api.DTOs;

public class PostResponse
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string? Caption { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; } = default!;
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
}

public class CreatePostRequest
{
    [Required]
    public IFormFile Image { get; set; } = default!;

    [StringLength(2200)]
    public string? Caption { get; set; }
}

public class PaginatedResponse<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}
