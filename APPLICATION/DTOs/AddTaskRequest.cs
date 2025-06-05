using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Task = DOMAIN.Models.Task;

namespace APPLICATION.DTOs;

public record AddTaskRequest()
{
    public int? UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Priority { get; set; }
}