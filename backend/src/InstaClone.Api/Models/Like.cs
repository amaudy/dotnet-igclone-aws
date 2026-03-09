namespace InstaClone.Api.Models;

public class Like
{
    public int Id { get; set; }
    public string UserId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
    public int PostId { get; set; }
    public Post Post { get; set; } = default!;
}
