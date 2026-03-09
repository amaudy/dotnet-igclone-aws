using System.Security.Claims;
using InstaClone.Api.Data;
using InstaClone.Api.DTOs;
using InstaClone.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstaClone.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/posts/{postId}/like")]
public class LikesController : ControllerBase
{
    private readonly AppDbContext _db;

    public LikesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Like(int postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var post = await _db.Posts.FindAsync(postId);
        if (post == null)
            return NotFound(new ErrorResponse { Message = "Post not found" });

        var existing = await _db.Likes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

        if (existing != null)
            return NoContent();

        _db.Likes.Add(new Like { UserId = userId, PostId = postId });
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Unlike(int postId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var like = await _db.Likes
            .FirstOrDefaultAsync(l => l.UserId == userId && l.PostId == postId);

        if (like == null)
            return NotFound(new ErrorResponse { Message = "Post not found" });

        _db.Likes.Remove(like);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
