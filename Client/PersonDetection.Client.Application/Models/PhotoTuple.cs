namespace PersonDetection.Client.Application.Models;

public class PhotoTuple
{
    public required Photo Original { get; set; }
    public required Photo Processed { get; set; }
}