using PersonDetection.Backend.Application.Models.Types;

namespace PersonDetection.Backend.Application.Common.Errors;

public class UserPasswordServiceError(string message, int statusCode) : Error(message, statusCode)
{
    public static UserPasswordServiceError InvalidPassword() 
        => new("Invalid password", ResultCodes.Unauthorized);
}