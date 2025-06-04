using System.Security.Claims;
using System.Text;
using DOMAIN.Models;
using INFRASTRUCTURE.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Task = System.Threading.Tasks.Task;

namespace APPLICATION.Services;

public class JwtService(
    IConfiguration configuration,
    TokenValidationParameters tokenValidationParameters,
    ILogger<JwtService> logger, AppDbContext context)
{
    public async Task<(string Token, DateTime expirationDate)> GenerateTokenAsync(AppUser user)
    {
        // generate claims then add standard claims then generate token
        try
        {
            var secretKey = configuration["Jwt:SecretKey"];
            var issuer = configuration["Jwt:ValidIssuer"];
            var audience = configuration["Jwt:ValidAudience"];
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var claims = await GenerateUserClaimsAsync(user);
            var claimsList = claims.ToList();
            var roleClaim = claimsList.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            var expiryMinutes = int.Parse(configuration["Jwt:ExpiryMinutes"]!);
            var expiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes);
            var fullClaims = AddStandardClaims(claimsList);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = issuer,
                Audience = audience,
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(fullClaims),
                NotBefore = DateTime.UtcNow
            };
            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            if (!(await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters)).IsValid)
            {
                throw new SecurityTokenValidationException("Generated jwt Failed Validation!");
            }

            return (token, expiresAt);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to generate JWT Token");
            throw;
        }
    } 
    private Task<List<Claim>> GenerateUserClaimsAsync(AppUser user)
    {
        var claims = new List<Claim>
        {
            new ("UserId", user.Id.ToString()),
            new (ClaimTypes.Name, user.UserName!),
            new (ClaimTypes.Email, user.Email!)
        };
        claims.Add(new Claim(ClaimTypes.Role, "User"));

        return Task.FromResult(claims);
    }
    private IEnumerable<Claim> AddStandardClaims(IEnumerable<Claim> claims)
    {
        var enhancedClaims = claims.ToList();

        if (enhancedClaims.All(c => c.Type != JwtRegisteredClaimNames.Jti))
        {
            enhancedClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        }

        if (enhancedClaims.All(c => c.Type != JwtRegisteredClaimNames.Iat))
        {
            enhancedClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, 
                DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()));
        }

        return enhancedClaims;
    }
    public async Task<RefreshToken> GenerateRefreshTokenAsync(int userId)
    {
        var refreshToken = new RefreshToken
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddHours(
                int.Parse(configuration["Jwt:RefreshTokenExpiryHours"] ?? "5"))
        };
        // ToDo: Removing Old Tokens
        // *note*: Consider implementing a scheduled cleanUp Service 
        
        return refreshToken;
    }
}