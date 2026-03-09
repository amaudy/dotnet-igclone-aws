namespace InstaClone.Api.Models;

public class Comment
{
    public int Id { get; set; }
    public string Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
    public int PostId { get; set; }
    public Post Post { get; set; } = default!;
}
