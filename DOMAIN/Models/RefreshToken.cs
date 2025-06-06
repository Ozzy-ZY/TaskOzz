using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOMAIN.Models;


public class RefreshToken
{
    [Key]
    public string Token { get; set; } = Guid.NewGuid().ToString();
    public int UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRevoked { get; set; }
    public string? ReplacedByToken { get; set; }
    public string? RevokeReason { get; set; }
    
    // Database property for manual override (nullable - null means "use computed value")
    public bool? IsActiveOverride { get; set; }
    
    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    
    [NotMapped]
    public bool IsActive
    {
        get => IsActiveOverride ?? (!IsRevoked && !IsExpired); // Use override if set, otherwise compute
        set => IsActiveOverride = value; // Setting this overrides the computed behavior
    }
}