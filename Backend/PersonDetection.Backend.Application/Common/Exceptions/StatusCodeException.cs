using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class StatusCodeException(string _message, int _statusCode) : Exception(_message)
{
    public int StatusCode { get; set; } = _statusCode;
}