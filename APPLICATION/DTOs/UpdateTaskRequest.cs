using Microsoft.AspNetCore.Http;

namespace APPLICATION.DTOs;

public record UpdateTaskRequest
{
    public int TaskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Priority { get; set; }
    public string? Status { get; set; }
    public bool? IsDone { get; set; }
    public IFormFile? IconFile { get; set; }
} 