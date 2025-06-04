using Microsoft.AspNetCore.Http;

namespace APPLICATION.DTOs;

public class UploadUserIconRequest
{
    public IFormFile File { get; set; }
    public int UserId { get; set; }
}

public class UploadTaskIconRequest
{
    public IFormFile File { get; set; }
    public int TaskId { get; set; }
}

public class DeleteUserIconRequest
{
    public int UserId { get; set; }
    public string IconPath { get; set; }
}

public class DeleteTaskIconRequest
{
    public int TaskId { get; set; }
    public string IconPath { get; set; }
} 