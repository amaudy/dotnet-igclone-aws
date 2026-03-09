using System.Security.Claims;
using InstaClone.Api.Data;
using InstaClone.Api.DTOs;
using InstaClone.Api.Models;
using InstaClone.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstaClone.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IImageService _imageService;

    public PostsController(AppDbContext db, IImageService imageService)
    {
        _db = db;
        _imageService = imageService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreatePostRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var imageUrl = await _imageService.SaveImageAsync(request.Image);

        var post = new Post
        {
            ImageUrl = imageUrl,
            Caption = request.Caption,
            UserId = userId
        };

        _db.Posts.Add(post);
        await _db.SaveChangesAsync();

        await _db.Entry(post).Reference(p => p.User).LoadAsync();

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, ToResponse(post));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? username = null)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var baseQuery = _db.Posts.AsQueryable();

        if (!string.IsNullOrEmpty(username))
            baseQuery = baseQuery.Where(p => p.User.UserName == username);

        var query = baseQuery.OrderByDescending(p => p.CreatedAt);

        var totalCount = await query.CountAsync();
        var posts = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PostResponse
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Caption = p.Caption,
                CreatedAt = p.CreatedAt,
                Username = p.User.UserName!,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count
            })
            .ToListAsync();

        return Ok(new PaginatedResponse<PostResponse>
        {
            Items = posts,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var post = await _db.Posts
            .Where(p => p.Id == id)
            .Select(p => new PostResponse
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Caption = p.Caption,
                CreatedAt = p.CreatedAt,
                Username = p.User.UserName!,
                LikeCount = p.Likes.Count,
                CommentCount = p.Comments.Count
            })
            .FirstOrDefaultAsync();

        if (post == null)
            return NotFound(new ErrorResponse { Message = "Post not found" });

        return Ok(post);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var post = await _db.Posts.FirstOrDefaultAsync(p => p.Id == id && p.UserId == userId);

        if (post == null)
            return NotFound(new ErrorResponse { Message = "Post not found" });

        _imageService.DeleteImage(post.ImageUrl);
        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    private static PostResponse ToResponse(Post post) => new()
    {
        Id = post.Id,
        ImageUrl = post.ImageUrl,
        Caption = post.Caption,
        CreatedAt = post.CreatedAt,
        Username = post.User.UserName!,
        LikeCount = post.Likes.Count,
        CommentCount = post.Comments.Count
    };
}
