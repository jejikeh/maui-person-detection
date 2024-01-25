using PersonDetection.Client.Infrastructure.Common;

namespace PersonDetection.Client.Configuration;

public class ClientConfiguration : IInfrastructureConfiguration
{
    public string PhotoProcessUrl { get; set; } = "http://localhost:5000/api/";
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromHours(1);
    public string DatabaseFileName { get; set; } = "PersonDetection.db";
    public bool SavePhotoToGallery { get; set; } = false;
}