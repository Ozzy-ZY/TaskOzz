namespace APPLICATION.DTOs;

public record GetTasksRequest
{
    public int UserId { get; set; }
    public string? FilterKey { get; set; }
    public string? SortKey { get; set; }
    public string? SortOrder { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}