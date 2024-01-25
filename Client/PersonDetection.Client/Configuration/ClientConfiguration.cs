using System;
using PersonDetection.Client.Infrastructure.Common;

namespace PersonDetection.Client.Configuration;

public class ClientConfiguration : IInfrastructureConfiguration
{
    public string PhotoProcessUrl { get; set; } = string.Empty;
    public TimeSpan CacheExpirationTime { get; set; } = TimeSpan.FromHours(1);
    public string DatabaseFileName { get; set; } = "PersonDetection.db";
    public bool SavePhotoToGallery { get; set; } = false;
}