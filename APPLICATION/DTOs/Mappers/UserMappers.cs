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

    public static MeResponse ToDto(this AppUser user)
    {
        return new MeResponse()
        {
            UserName = user.UserName,
            Email = user.Email,
            IconPath = user.IconUrl
        };
    }
}