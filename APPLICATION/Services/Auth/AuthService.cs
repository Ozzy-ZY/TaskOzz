using APPLICATION.DTOs;
using APPLICATION.DTOs.Mappers;
using APPLICATION.Validator;
using DOMAIN.Models;
using INFRASTRUCTURE.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace APPLICATION.Services;

public partial class AuthService(
    AppDbContext context,
    LoginValidator loginValidator,
    RegisterValidator registerValidator,
    PasswordService passwordService,
    ILogger<AuthService> logger,
    JwtService jwtService)
{
    private readonly AppDbContext _context = context;
    private readonly LoginValidator _loginValidator = loginValidator;
    private readonly RegisterValidator _registerValidator = registerValidator;
    private readonly PasswordService _passwordService = passwordService;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly JwtService _jwtService = jwtService;
    
    public async Task<Result> RegisterUserAsync(RegisterRequest registerRequest)
    {
        try
        {
            var existingUser = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
            
            if (existingUser is not null)
            {
                return new Result
                { 
                    StatusCode = (int)AuthFlags.UserAlreadyExists,
                    Message = "User with this email already exists",
                    Data = registerRequest
                };
            }
            
            var hashedPassword = _passwordService.HashPassword(registerRequest.Password);
            var newUser = registerRequest.ToAppUser();
            newUser.PasswordHash = hashedPassword;

            await _context.Users.AddAsync(newUser);
            var saveResult = await _context.SaveChangesAsync();
        
            if (saveResult > 0)
            {
                return new Result
                {
                    StatusCode = (int)AuthFlags.Success,
                    Message = "User registered successfully",
                    Data = registerRequest
                };
            }

            return new Result
            {
                StatusCode = (int)AuthFlags.DataBaseError,
                Message = "Failed to save user to database",
                Data = registerRequest
            };
        }
        catch (Exception ex)
        {
            return new Result
            {
                StatusCode = (int)AuthFlags.Exception,
                Message = $"An error occurred during registration",
                Data = registerRequest
            };
        }
    }

    public async Task<Result> LoginUserAsync(LoginRequest loginRequest)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user is null)
            {
                return new Result
                {
                    StatusCode = (int)AuthFlags.InvalidCredentials,
                    Message = "please Enter Valid Credentials",
                    Data = loginRequest
                };
            }

            var res = _passwordService
                .VerifyHashedPassword(user.PasswordHash, loginRequest.Password);
            if (!res)
            {
                return new Result
                {
                    StatusCode = (int)AuthFlags.InvalidCredentials,
                    Message = "please Enter Valid Credentials",
                    Data = loginRequest
                };
            }

            // generate access & Refresh tokens and return them
            var (token, expiry) = await _jwtService.GenerateTokenAsync(user);
            _logger.LogInformation("Generated token: {Token}", token);
            
            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id);
            await _context.RefreshTokens.AddAsync(refreshToken);
            var authResult = new AuthResult()
            {
                AccessToken = token,
                AccessTokenExpiration = expiry,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpiration = refreshToken.ExpiresAt
            };
            user.LastLogin = DateTime.UtcNow;
            if (await _context.SaveChangesAsync() <= 0)
            {
                return new Result()
                {
                    StatusCode = (int)AuthFlags.DataBaseError,
                    Message = "Failed to save refresh token to database",
                    Data = loginRequest,
                };
            }
            return new Result
            {
                StatusCode = (int)AuthFlags.Success,
                Message = "Login Successful",
                Data = authResult
            };
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Exception in Login {LoginRequest}", loginRequest);
            return new Result()
            {
                StatusCode = (int)AuthFlags.Exception,
                Message = "Internal Error"
            };
        }
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user is null)
        {
            return new Result()
            {
                StatusCode = (int)AuthFlags.InvalidCredentials,
                Message = "Invalid Credentials",
                Data = request
            };
        }
        var newHash = _passwordService.HashPassword(request.NewPassword);
        user.PasswordHash = newHash;
        if(await _context.SaveChangesAsync() <=0)
        {
            return new Result()
            {
                StatusCode = (int)AuthFlags.DataBaseError,
                Message = "Failed to save user to database",
                Data = request
            };
        }
        return new Result()
        {
            StatusCode = (int)AuthFlags.Success,
            Message = "Password Changed Successfully"
        };
    }

    public async Task<Result> RefreshAccessTokenAsync(RefreshOrRevokeTokenRequest request)
    {
        var oldToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == request.RefreshToken && t.IsActive);
        if (oldToken is null)
        {
            return new Result()
            {
                StatusCode = (int)AuthFlags.InvalidToken,
                Message = "Refresh Token is not Valid",
            };
        }
        var newRefreshToken = await _jwtService.GenerateRefreshTokenAsync(oldToken.UserId);
        var user = await _context.Users.FirstOrDefaultAsync(u=> u.Id == oldToken.UserId);
        var (token, expiry) = await _jwtService.GenerateTokenAsync(user!);
        _context.RefreshTokens.Remove(oldToken);
        await _context.RefreshTokens.AddAsync(newRefreshToken);
        if (await _context.SaveChangesAsync() <= 0)
        {
            return new Result()
            {
                StatusCode = (int)AuthFlags.DataBaseError,
                Message = "Failed to save refresh token to database",
                Data = request,
            };
        }
        return new Result()
        {
            StatusCode = (int)AuthFlags.Success,
            Message = "Token Refreshed Successfully",
            Data = new AuthResult()
            {
                AccessToken = token,
                AccessTokenExpiration = expiry,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiration = newRefreshToken.ExpiresAt
            }
        };
    }

    public async Task<Result> RevokeRefreshTokenAsync(string refreshToken)
    {
        try
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken && t.IsActive);

            if (token is null)
            {
                return new Result
                {
                    StatusCode = (int)AuthFlags.InvalidToken,
                    Message = "Invalid or expired refresh token"
                };
            }

            token.IsRevoked = true;
            token.RevokeReason = "User logged out";
            token.ReplacedByToken = null;

            if (await _context.SaveChangesAsync() <= 0)
            {
                return new Result
                {
                    StatusCode = (int)AuthFlags.DataBaseError,
                    Message = "Failed to revoke refresh token"
                };
            }

            return new Result
            {
                StatusCode = (int)AuthFlags.Success,
                Message = "Token revoked successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revoking refresh token");
            return new Result
            {
                StatusCode = (int)AuthFlags.Exception,
                Message = "An error occurred while revoking the token"
            };
        }
    }
}