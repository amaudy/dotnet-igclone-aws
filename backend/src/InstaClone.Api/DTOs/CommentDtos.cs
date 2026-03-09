using System.ComponentModel.DataAnnotations;

namespace InstaClone.Api.DTOs;

public class CommentResponse
{
    public int Id { get; set; }
    public string Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; } = default!;
}

public class CreateCommentRequest
{
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Text { get; set; } = default!;
}
