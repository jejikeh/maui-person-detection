using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class InvalidCredentialsException(string _message = "Invalid credentials.") : StatusCodeException(_message, StatusCodes.Status401Unauthorized)
{
}
