using System.IO;
using APPLICATION.DTOs;
using DOMAIN.Models;
using INFRASTRUCTURE.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APPLICATION.Services;

public class ImageService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ImageService> _logger;
    private readonly AppDbContext _context;
    private const string UserIconsPath = "images/users";
    private const string TaskIconsPath = "images/tasks";
    private const string DefaultUserIcon = "default-user.png";
    private const string DefaultTaskIcon = "default-task.png";

    public ImageService(
        IWebHostEnvironment environment, 
        ILogger<ImageService> logger,
        AppDbContext context)
    {
        _environment = environment;
        _logger = logger;
        _context = context;
        InitializeDirectories();
    }

    private void InitializeDirectories()
    {
        var userIconsDir = Path.Combine(_environment.WebRootPath, UserIconsPath);
        var taskIconsDir = Path.Combine(_environment.WebRootPath, TaskIconsPath);

        if (!Directory.Exists(userIconsDir))
        {
            Directory.CreateDirectory(userIconsDir);
        }

        if (!Directory.Exists(taskIconsDir))
        {
            Directory.CreateDirectory(taskIconsDir);
        }
    }

    public async Task<Result> UploadUserIconAsync(UploadUserIconRequest request)
    {
        try
        {
            if (!IsValidImageFile(request.File))
            {
                return new Result
                {
                    StatusCode = 400,
                    Message = "Invalid image file. Please upload a valid image file (jpg, jpeg, png, or gif)."
                };
            }

            var user = await _context.Users.FirstOrDefaultAsync(u=> u.Id == request.UserId);
            if (user is null)
            {
                return new Result
                {
                    StatusCode = 404,
                    Message = "User not found"
                };
            }

            // Delete old icon if exists
            if (!string.IsNullOrEmpty(user.IconUrl))
            {
                await DeleteUserIconAsync(new DeleteUserIconRequest 
                { 
                    UserId = request.UserId, 
                    IconPath = user.IconUrl 
                });
            }

            var fileName = $"{request.UserId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(request.File.FileName)}";
            var filePath = Path.Combine(_environment.WebRootPath, UserIconsPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var iconPath = $"/{UserIconsPath}/{fileName}";
            user.IconUrl = iconPath;
            await _context.SaveChangesAsync();

            return new Result
            {
                StatusCode = 200,
                Message = "User icon uploaded successfully",
                Data = new { IconPath = iconPath }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading user icon for user {UserId}", request.UserId);
            return new Result
            {
                StatusCode = 500,
                Message = "An error occurred while uploading the user icon"
            };
        }
    }

    public async Task<Result> UploadTaskIconAsync(UploadTaskIconRequest request)
    {
        try
        {
            if (!IsValidImageFile(request.File))
            {
                return new Result
                {
                    StatusCode = 400,
                    Message = "Invalid image file. Please upload a valid image file (jpg, jpeg, or png)."
                };
            }

            var task = await _context.Tasks.FirstOrDefaultAsync(t=> t.Id == request.TaskId);
            if (task is null)
            {
                return new Result
                {
                    StatusCode = 404,
                    Message = "Task not found"
                };
            }

            // Delete old icon if exists
            if (!string.IsNullOrEmpty(task.IconUrl))
            {
                await DeleteTaskIconAsync(new DeleteTaskIconRequest 
                { 
                    TaskId = request.TaskId, 
                    IconPath = task.IconUrl 
                });
            }

            var fileName = $"{request.TaskId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(request.File.FileName)}";
            var filePath = Path.Combine(_environment.WebRootPath, TaskIconsPath, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var iconPath = $"/{TaskIconsPath}/{fileName}";
            task.IconUrl = iconPath;
            await _context.SaveChangesAsync();

            return new Result
            {
                StatusCode = 200,
                Message = "Task icon uploaded successfully",
                Data = new { IconPath = iconPath }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading task icon for task {TaskId}", request.TaskId);
            return new Result
            {
                StatusCode = 500,
                Message = "An error occurred while uploading the task icon"
            };
        }
    }

    public async Task<Result> DeleteUserIconAsync(DeleteUserIconRequest request)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user is null)
            {
                return new Result
                {
                    StatusCode = 404,
                    Message = "User not found"
                };
            }

            if (string.IsNullOrEmpty(request.IconPath) || request.IconPath == GetDefaultUserIconPath())
            {
                return new Result
                {
                    StatusCode = 200,
                    Message = "No icon to delete"
                };
            }

            var fullPath = Path.Combine(_environment.WebRootPath, request.IconPath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            user.IconUrl = GetDefaultUserIconPath();
            await _context.SaveChangesAsync();

            return new Result
            {
                StatusCode = 200,
                Message = "User icon deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user icon for user {UserId}", request.UserId);
            return new Result
            {
                StatusCode = 500,
                Message = "An error occurred while deleting the user icon"
            };
        }
    }

    public async Task<Result> DeleteTaskIconAsync(DeleteTaskIconRequest request)
    {
        try
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t=> t.Id == request.TaskId);
            if (task is null)
            {
                return new Result
                {
                    StatusCode = 404,
                    Message = "Task not found"
                };
            }

            if (string.IsNullOrEmpty(request.IconPath) || request.IconPath == GetDefaultTaskIconPath())
            {
                return new Result
                {
                    StatusCode = 200,
                    Message = "No icon to delete"
                };
            }

            var fullPath = Path.Combine(_environment.WebRootPath, request.IconPath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            task.IconUrl = GetDefaultTaskIconPath();
            await _context.SaveChangesAsync();

            return new Result
            {
                StatusCode = 200,
                Message = "Task icon deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task icon for task {TaskId}", request.TaskId);
            return new Result
            {
                StatusCode = 500,
                Message = "An error occurred while deleting the task icon"
            };
        }
    }

    public async Task<Result> GetUserIconAsync(int userId)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=> u.Id == userId);
            if (user is null)
            {
                return new Result
                {
                    StatusCode = 404,
                    Message = "User not found"
                };
            }

            var iconPath = string.IsNullOrEmpty(user.IconUrl) 
                ? GetDefaultUserIconPath() 
                : user.IconUrl;

            return new Result
            {
                StatusCode = 200,
                Data = new { IconPath = iconPath }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user icon for user {UserId}", userId);
            return new Result
            {
                StatusCode = 500,
                Message = "An error occurred while getting the user icon"
            };
        }
    }

    public async Task<Result> GetTaskIconAsync(int taskId)
    {
        try
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t=> t.Id == taskId);
            if (task is null)
            {
                return new Result
                {
                    StatusCode = 404,
                    Message = "Task not found"
                };
            }

            var iconPath = string.IsNullOrEmpty(task.IconUrl) 
                ? GetDefaultTaskIconPath() 
                : task.IconUrl;

            return new Result
            {
                StatusCode = 200,
                Data = new { IconPath = iconPath }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task icon for task {TaskId}", taskId);
            return new Result
            {
                StatusCode = 500,
                Message = "An error occurred while getting the task icon"
            };
        }
    }

    public string GetDefaultUserIconPath() => $"/{UserIconsPath}/{DefaultUserIcon}";
    public string GetDefaultTaskIconPath() => $"/{TaskIconsPath}/{DefaultTaskIcon}";

    private bool IsValidImageFile(IFormFile file)
    {
        if (file is null || file.Length == 0)
            return false;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        return allowedExtensions.Contains(extension) && 
               file.ContentType.StartsWith("image/");
    }
} 