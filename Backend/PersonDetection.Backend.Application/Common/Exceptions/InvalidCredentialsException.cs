using Microsoft.AspNetCore.Http;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class InvalidCredentialsException() : StatusCodeException("Invalid credentials", StatusCodes.Status401Unauthorized);
