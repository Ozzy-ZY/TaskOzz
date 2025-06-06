namespace APPLICATION.DTOs;

public record MeResponse
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string IconPath { get; set; }
}