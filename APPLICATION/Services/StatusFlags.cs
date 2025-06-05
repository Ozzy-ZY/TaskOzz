namespace APPLICATION.Services;

public enum StatusFlags
{
    Success = 1,
    UserAlreadyExists = 100,
    InvalidCredentials = 101,
    InvalidToken = 102,
    DataBaseError = 500,
    Exception = 999
}