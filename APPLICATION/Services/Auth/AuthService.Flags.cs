namespace APPLICATION.Services;

public partial class AuthService
{
    public enum AuthFlags:int
    {
        Success = 1,
        UserAlreadyExists = 100,
        InvalidCredentials = 101,
        InvalidToken = 102,
        DataBaseError = 500,
        Exception = 999
    }
}