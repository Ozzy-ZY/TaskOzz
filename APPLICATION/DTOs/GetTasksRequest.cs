namespace APPLICATION.DTOs;

public record GetTasksRequest
{
    public int UserId { get; set; }
    public string? FilterKey { get; set; }
}