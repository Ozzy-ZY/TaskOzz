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
                StatusCode = (int)StatusFlags.DataBaseError,
                Message = "DataBase Error",
                Data = request
            };
        }

        return new Result
        {
            StatusCode = (int)StatusFlags.Success,
            Message = "Task Added!",
        };
    }

    public Task<Result> GetAllTasksForUser(int userId)
    {
        var tasks = _context.Tasks.AsQueryable()
            .Where(t => t.UserId == userId)
            .AsNoTracking();
        return Task.FromResult(new Result
        {
            StatusCode = (int)StatusFlags.Success,
            Message = "Tasks Found",
            Data = tasks.ToList()
        });
    }
}