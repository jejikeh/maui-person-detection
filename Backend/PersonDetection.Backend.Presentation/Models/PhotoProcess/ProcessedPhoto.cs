namespace PersonDetection.Backend.Presentation.Models.PhotoProcess;

public class ProcessedPhoto
{
    public string PhotoBase64 { get; set; } = string.Empty;
    public int PersonsCount { get; set; }
}