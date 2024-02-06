namespace PersonDetection.Backend.Application.Common;

public static class ResultCodes
{
    public const int InternalServerError = 500;
    
    public const int BadRequest = 400;
    public const int Unauthorized = 401;
    public const int Forbidden = 403;
    public const int NotFound = 404;
    public const int Conflict = 409;

    public const int Created = 201;
}