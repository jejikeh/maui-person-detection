using FluentValidation;
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
            var problemDetails = new ProblemDetails
            {
                Title = "An error occurred",
                Status = statusCodeException.StatusCode,
                Detail = statusCodeException.Message,
            };

            context.Response.StatusCode = statusCodeException.StatusCode;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
        catch (ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, x => x.Select(y => y.ErrorMessage).ToArray());
            
            var problemDetails = new ValidationProblemDetails
            {
                Title = "An error occurred",
                Status = StatusCodes.Status400BadRequest,
                Detail = validationException.Message,
                Errors = errors,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
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