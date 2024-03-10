namespace PersonDetection.Backend.Infrastructure.Models;

public class PhotoInBucket
{
    public Guid Id { get; set; }
    
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
}