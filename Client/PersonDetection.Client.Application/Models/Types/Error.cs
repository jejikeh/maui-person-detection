namespace PersonDetection.Client.Application.Models.Types;

public class Error(string message)
{
    public string Message { get; set; } = message;
}