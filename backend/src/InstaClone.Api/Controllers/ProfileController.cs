using System.Security.Claims;
using InstaClone.Api.DTOs;
using InstaClone.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InstaClone.Api.Controllers;

[ApiController]
[Route("api/profiles")]
public class ProfileController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public ProfileController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return NotFound(new ErrorResponse { Message = "User not found" });

        return Ok(new ProfileResponse
        {
            Username = user.UserName!,
            DisplayName = user.DisplayName,
            Bio = user.Bio,
            AvatarUrl = user.AvatarUrl,
            PostCount = 0
        });
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null)
            return Unauthorized(new ErrorResponse { Message = "User not found" });

        if (request.DisplayName != null) user.DisplayName = request.DisplayName;
        if (request.Bio != null) user.Bio = request.Bio;
        if (request.AvatarUrl != null) user.AvatarUrl = request.AvatarUrl;

        await _userManager.UpdateAsync(user);

        return Ok(new ProfileResponse
        {
            Username = user.UserName!,
            DisplayName = user.DisplayName,
            Bio = user.Bio,
            AvatarUrl = user.AvatarUrl,
            PostCount = 0
        });
    }
}
