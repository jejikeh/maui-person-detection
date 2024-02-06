using PersonDetection.Backend.Application.Models.Types;

namespace PersonDetection.Backend.Application.Common.Errors;

public class UserServiceError(string message, int statudCode) : Error(message, statudCode)
{
    public static UserServiceError UserNotFoundByEmail(string email)
        => new($"User with email '{email}' not found", ResultCodes.NotFound);
    
    public static UserServiceError UserNotFoundById(string id)
        => new($"User with id '{id}' not found", ResultCodes.NotFound);
    
    public static UserServiceError UserAlreadyExists(string email, string id)
        => new($"User with email '{email}:{id}' already exists", ResultCodes.Conflict);
}