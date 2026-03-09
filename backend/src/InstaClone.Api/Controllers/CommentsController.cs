using System.Security.Claims;
using InstaClone.Api.Data;
using InstaClone.Api.DTOs;
using InstaClone.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstaClone.Api.Controllers;

[ApiController]
public class CommentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public CommentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("api/posts/{postId}/comments")]
    public async Task<IActionResult> GetComments(int postId)
    {
        var post = await _db.Posts.FindAsync(postId);
        if (post == null)
            return NotFound(new ErrorResponse { Message = "Post not found" });

        var comments = await _db.Comments
            .Include(c => c.User)
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentResponse
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                Username = c.User.UserName!
            })
            .ToListAsync();

        return Ok(comments);
    }

    [Authorize]
    [HttpPost("api/posts/{postId}/comments")]
    public async Task<IActionResult> CreateComment(int postId, [FromBody] CreateCommentRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var post = await _db.Posts.FindAsync(postId);
        if (post == null)
            return NotFound(new ErrorResponse { Message = "Post not found" });

        var comment = new Comment
        {
            Text = request.Text,
            UserId = userId,
            PostId = postId
        };

        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();

        await _db.Entry(comment).Reference(c => c.User).LoadAsync();

        return CreatedAtAction(nameof(GetComments), new { postId }, new CommentResponse
        {
            Id = comment.Id,
            Text = comment.Text,
            CreatedAt = comment.CreatedAt,
            Username = comment.User.UserName!
        });
    }

    [Authorize]
    [HttpDelete("api/comments/{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var comment = await _db.Comments.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (comment == null)
            return NotFound(new ErrorResponse { Message = "Comment not found" });

        _db.Comments.Remove(comment);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
