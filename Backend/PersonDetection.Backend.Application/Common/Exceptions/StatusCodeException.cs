using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PersonDetection.Backend.Application.Common.Exceptions;

public class StatusCodeException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; set; } = statusCode;

    public virtual async Task ToContextAsync(HttpContext context)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred",
            Status = StatusCode,
            Detail = Message,
        };

        context.Response.StatusCode = StatusCode;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}