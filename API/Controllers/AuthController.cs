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
        if (result.StatusCode == (int)StatusFlags.Success)
            return Ok(result);
        
        return BadRequest(result);
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginUserAsync(request);
        if (result.StatusCode == (int)StatusFlags.Success)
            return Ok(result);
        
        return BadRequest(result);
    }

    [HttpGet("Me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        return Ok(User);
    }
    
    [HttpPut("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _authService.ChangePasswordAsync(request);
        if (result.StatusCode == (int)StatusFlags.Success)
            return Ok(result);
        
        return BadRequest(result);
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshOrRevokeTokenRequest request)
    {
        var result = await _authService.RefreshAccessTokenAsync(request);
        if (result.StatusCode == (int)StatusFlags.Success)
            return Ok(result);
        
        return BadRequest(result);
    }

    [HttpPost("RevokeToken")]
    [Authorize]
    public async Task<IActionResult> RevokeToken([FromBody] RefreshOrRevokeTokenRequest request)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
        if (result.StatusCode == (int)StatusFlags.Success)
            return Ok(result);
        
        return BadRequest(result);
    }
}