using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class StatusCodeException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; set; } = statusCode;
}