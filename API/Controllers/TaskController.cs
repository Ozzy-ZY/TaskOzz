using APPLICATION.DTOs;
using APPLICATION.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TaskController(TaskService taskService) : ControllerBase
{
    private readonly TaskService _taskService = taskService;

    [HttpPost("")]
    [Authorize]
    public async Task<IActionResult> AddTask(AddTaskRequest request)
    {
        var result = await _taskService.AddTaskToUserAsync(request);
        if (result.StatusCode == (int)StatusFlags.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpGet("")]
    [Authorize]
    public async Task<IActionResult> GetTasks()
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value!);
        var result = await _taskService.GetAllTasksForUser(userId);
        return Ok(result);
    }
}