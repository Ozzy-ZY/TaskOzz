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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddTask(AddTaskRequest request)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value!);
        request.UserId = userId;
        
        var result = await _taskService.AddTaskToUserAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetTasks(string? filterKey, string? sortKey = "Id", string? sortOrder = "asc")
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value!);
        var req = new GetTasksRequest{UserId = userId, FilterKey = filterKey,SortKey = sortKey, SortOrder = sortOrder };
        var result = await _taskService.GetAllTasksForUser(req);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{taskId}")]
    [Authorize]
    public async Task<IActionResult> GetTask(int taskId)
    {
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value!);
        var result = await _taskService.GetTaskById(taskId, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateTask([FromForm] UpdateTaskRequest request)
    {
        var result = await _taskService.UpdateTaskAsync(request);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{taskId}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var result = await _taskService.DeleteTaskAsync(new DeleteTaskRequest { TaskId = taskId });
        return StatusCode(result.StatusCode, result);
    }
}