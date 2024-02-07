using PersonDetection.Backend.Infrastructure.Common;

namespace PersonDetection.Backend.Infrastructure.Models.Identity;

public class RefreshToken(string ownerId, DateTime expiredAt, int contentLength = 10)
{
    public string Content { get; } = Utils.GenerateStringByRandom(contentLength);
    public DateTime CreatedAt { get; } = DateTime.Now;
    public DateTime ExpiredAt { get; } = expiredAt;
    public bool IsExpired => DateTime.Now > ExpiredAt;
}