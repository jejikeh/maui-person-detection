using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application.Common.Exceptions;

namespace PersonDetection.Backend.Web.Middlewares;

public class GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (StatusCodeException statusCodeException)
        {
            logger.LogError(statusCodeException, statusCodeException.Message);
            await statusCodeException.ToContextAsync(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, exception.Message);
            var problemDetails = new ProblemDetails
            {
                Title = "Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = exception.Message,
            };
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}