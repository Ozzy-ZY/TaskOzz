using APPLICATION.DTOs;
using APPLICATION.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(AuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var result = await _authService.RegisterUserAsync(request);
        if (result.StatusCode == (int)AuthService.AuthFlags.Success)
            return Ok(result);
        
        return BadRequest(result);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginUserAsync(request);
        if (result.StatusCode == (int)AuthService.AuthFlags.Success)
            return Ok(result);
        
        return BadRequest(result);
    }

    [HttpGet("Me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        return Ok(User);
    }
    [HttpGet("Current-User")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CurrentUser()
    {
        _logger.LogInformation("CurrentUser endpoint called");
        _logger.LogInformation("Authorization header: {Header}", Request.Headers["Authorization"].ToString());
        _logger.LogInformation("User authenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
        _logger.LogInformation("User claims: {Claims}", string.Join(", ", User.Claims.Select(c => $"{c.Type}: {c.Value}")));
        
        var user = User.Claims.FirstOrDefault(c => c.Type == "UserId");
        if (user != null) return Ok(user);
        
        _logger.LogWarning("UserId claim not found in token");
        return Unauthorized("Invalid token: UserId claim not found");
    }
}