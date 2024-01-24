namespace PersonDetection.Client.Application.Models;

public class PhotoToProcess
{
    public string Photo { get; set; } = string.Empty;
    
    public static implicit operator PhotoToProcess(string base64) => new() { Photo = base64 };
}