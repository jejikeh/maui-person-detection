using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class InvalidCredentialsException(string message = "Invalid credentials.") : StatusCodeException(message, StatusCodes.Status401Unauthorized)
{
}
