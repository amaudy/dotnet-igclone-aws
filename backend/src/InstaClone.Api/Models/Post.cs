namespace InstaClone.Api.Models;

public class Post
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = default!;
    public string? Caption { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
