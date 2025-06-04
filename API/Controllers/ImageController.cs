using APPLICATION.DTOs;
using APPLICATION.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImageController(ImageService imageService, ILogger<ImageController> logger) : ControllerBase
{
    private readonly ILogger<ImageController> _logger = logger;

    [HttpGet("user-icon/")]
    public async Task<IActionResult> GetUserIcon()
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value!);
        var result = await imageService.GetUserIconAsync(userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("task-icon/{taskId}")]
    public async Task<IActionResult> GetTaskIcon(int taskId)
    {
        var result = await imageService.GetTaskIconAsync(taskId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("user-icon")]
    public async Task<IActionResult> UploadUserIcon([FromForm] UploadUserIconRequest request)
    {
        var result = await imageService.UploadUserIconAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("task-icon")]
    public async Task<IActionResult> UploadTaskIcon([FromForm] UploadTaskIconRequest request)
    {
        var result = await imageService.UploadTaskIconAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("user-icon")]
    public async Task<IActionResult> DeleteUserIcon([FromBody] DeleteUserIconRequest request)
    {
        var result = await imageService.DeleteUserIconAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("task-icon")]
    public async Task<IActionResult> DeleteTaskIcon([FromBody] DeleteTaskIconRequest request)
    {
        var result = await imageService.DeleteTaskIconAsync(request);
        return StatusCode(result.StatusCode, result);
    }
} 