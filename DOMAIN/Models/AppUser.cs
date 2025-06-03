using Microsoft.AspNetCore.Identity;

namespace DOMAIN.Models;

public class AppUser
{
    public int Id {get; init;}
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string IconUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }
    
    public string? GoogleId { get; set; }
    // nav prop
    public ICollection<Task> Tasks { get; set; } = (List<Task>) [];
}