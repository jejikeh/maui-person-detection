namespace PersonDetection.Client.Application.Models;

public class ProcessedPhoto
{
    public Guid Id { get; set; }
    public string OriginalPhoto { get; set; } = string.Empty;
    public string NewPhoto { get; set; } = string.Empty;
}