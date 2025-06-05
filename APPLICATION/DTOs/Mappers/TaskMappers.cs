using Task = DOMAIN.Models.Task;

namespace APPLICATION.DTOs.Mappers;

public static class TaskMappers
{
    public static Task ToModel(this AddTaskRequest request)
    {
        return new Task()
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            UserId = request.UserId,

        };
    }
}