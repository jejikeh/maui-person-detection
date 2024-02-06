using Microsoft.AspNetCore.Http;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class StatusCodeException(string message, int statusCode) : Exception
{
    public int StatusCode { get; set; } = statusCode;
}