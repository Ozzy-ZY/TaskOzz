namespace DOMAIN.Models;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = "Hanz the Banz Fast!";
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "ToDo";
    public string Priority { get; set; } = "High";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string IconUrl { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public int UserId { get; set; }
    //Nav prop
    public AppUser User { get; set; }
}