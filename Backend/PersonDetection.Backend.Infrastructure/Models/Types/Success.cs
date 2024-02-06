namespace PersonDetection.Backend.Application.Models.Types;

public class Success(string message = "Success", int statusCode = 200)
{
    public string Message { get; set; } = message;
    public int StatusCode { get; set; } = statusCode;
}