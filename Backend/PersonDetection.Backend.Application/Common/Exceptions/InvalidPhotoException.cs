using Microsoft.AspNetCore.Http;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class InvalidPhotoException() : StatusCodeException("Invalid photo.", StatusCodes.Status400BadRequest);
