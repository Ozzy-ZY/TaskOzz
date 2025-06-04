using DOMAIN.Models;
using Microsoft.AspNetCore.Identity;

namespace APPLICATION.Services;

public class PasswordService
{
    private readonly PasswordHasher<AppUser> _passwordHasher = new();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null!, password);
    }
    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(null!, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
    }
    
}