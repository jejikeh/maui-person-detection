namespace PersonDetection.Backend.Infrastructure.Models;

public class PhotoInBucket
{
    public int Id { get; set; }

    public string PhotoName { get; set; } = string.Empty;

    public string OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}