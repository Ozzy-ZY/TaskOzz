using DOMAIN.Models;

namespace APPLICATION.DTOs.Mappers;

public static class UserMappers
{
    public static AppUser ToAppUser(this RegisterRequest request)
    {
        return new AppUser
        {
            UserName = request.UserName,
            Email = request.Email.ToLower(),
            PasswordHash = request.Password
        };
    }
}