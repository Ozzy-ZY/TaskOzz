using APPLICATION.DTOs;
using APPLICATION.DTOs.Mappers;
using INFRASTRUCTURE.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace APPLICATION.Services;

public class TaskService(AppDbContext context, ImageService imageService)
{
    private readonly AppDbContext _context = context;
    private readonly ImageService _imageService = imageService;

    public async Task<Result> AddTaskToUserAsync(AddTaskRequest request)
    {
        var task = request.ToModel();
        task.IconUrl = _imageService.GetDefaultTaskIconPath();
        await _context.Tasks.AddAsync(task);
        if (await _context.SaveChangesAsync() <= 0)
        {
            return new Result
            {
                StatusCode = (int)StatusFlags.InternalServerError,
                Message = "Failed to add task",
                Data = request
            };
        }

        return new Result
        {
            StatusCode = (int)StatusFlags.Created,
            Message = "Task added successfully",
            Data = task
        };
    }

    public async Task<Result> GetAllTasksForUser(GetTasksRequest req)
    {
        var tasks = _context.Tasks
            .Where(t=> t.UserId == req.UserId);;
        if (!string.IsNullOrWhiteSpace(req.FilterKey))
        {
            tasks = tasks
                .Where(t => t.Title.Contains(req.FilterKey) || t.Description.Contains(req.FilterKey));
        }
        
        return new Result
        {
            StatusCode = (int)StatusFlags.Success,
            Message = "Tasks retrieved successfully",
            Data = await tasks.ToListAsync()
        };
    }

    public async Task<Result> GetTaskById(int taskId, int userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

        if (task is null)
        {
            return new Result
            {
                StatusCode = (int)StatusFlags.NotFound,
                Message = "Task not found"
            };
        }

        return new Result
        {
            StatusCode = (int)StatusFlags.Success,
            Message = "Task retrieved successfully",
            Data = task
        };
    }

    public async Task<Result> UpdateTaskAsync(UpdateTaskRequest request)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);
        if (task is null)
        {
            return new Result
            {
                StatusCode = (int)StatusFlags.NotFound,
                Message = "Task not found"
            };
        }

        // Update task properties if provided
        if (request.Title != null)
            task.Title = request.Title;
        if (request.Description != null)
            task.Description = request.Description;
        if (request.Priority != null)
            task.Priority = request.Priority;
        if (request.Status != null)
            task.Status = request.Status;
        if (request.IsDone.HasValue)
            task.IsDone = request.IsDone.Value;

        // Update icon if provided
        if (request.IconFile != null)
        {
            var iconResult = await _imageService.UploadTaskIconAsync(new UploadTaskIconRequest
            {
                TaskId = task.Id,
                File = request.IconFile
            });

            if (iconResult.StatusCode != (int)StatusFlags.Success)
            {
                return iconResult;
            }
        }

        task.UpdatedAt = DateTime.UtcNow;
        
        if (await _context.SaveChangesAsync() <= 0)
        {
            return new Result
            {
                StatusCode = (int)StatusFlags.InternalServerError,
                Message = "Failed to update task"
            };
        }

        return new Result
        {
            StatusCode = (int)StatusFlags.Success,
            Message = "Task updated successfully",
            Data = task
        };
    }

    public async Task<Result> DeleteTaskAsync(DeleteTaskRequest request)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);
        if (task is null)
        {
            return new Result
            {
                StatusCode = (int)StatusFlags.NotFound,
                Message = "Task not found"
            };
        }

        // Delete task icon if it exists and is not the default icon
        if (!string.IsNullOrEmpty(task.IconUrl) && task.IconUrl != _imageService.GetDefaultTaskIconPath())
        {
            var deleteIconResult = await _imageService.DeleteTaskIconAsync(new DeleteTaskIconRequest
            {
                TaskId = task.Id,
                IconPath = task.IconUrl
            });

            if (deleteIconResult.StatusCode != (int)StatusFlags.Success)
            {
                return deleteIconResult;
            }
        }

        _context.Tasks.Remove(task);
        
        if (await _context.SaveChangesAsync() <= 0)
        {
            return new Result
            {
                StatusCode = (int)StatusFlags.InternalServerError,
                Message = "Failed to delete task"
            };
        }

        return new Result
        {
            StatusCode = (int)StatusFlags.NoContent,
            Message = "Task deleted successfully"
        };
    }
}