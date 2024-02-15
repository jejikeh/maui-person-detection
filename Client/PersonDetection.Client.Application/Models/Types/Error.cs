namespace PersonDetection.Client.Application.Models.Types;

public class Error(string _message)
{
    public string Message { get; } = _message;
}