namespace APPLICATION.Services;

public enum StatusFlags
{
    // Success codes
    Success = 200,
    Created = 201,
    NoContent = 204,

    // Client error codes
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    ValidationError = 422,

    // Server error codes
    InternalServerError = 500,
    ServiceUnavailable = 503
}