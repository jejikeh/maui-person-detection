namespace PersonDetection.Backend.Application.Models.Types;

public class Error(string message, int statusCode)
{
    public string Message { get; set; } = message;
    public int StatusCode { get; set; } = statusCode;
}