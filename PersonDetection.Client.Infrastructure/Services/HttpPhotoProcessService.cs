using PersonDetection.Client.Common.Services;
using PersonDetection.Client.Infrastructure.Common;

namespace PersonDetection.Client.Infrastructure.Services;

public class HttpPhotoProcessService(
    HttpClientProvider httpClient, 
    IConnectivity connectivity,
    IInfrastructureConfiguration configuration) : IPhotoProcessService
{

    public Task<byte[]> ProcessPhoto(byte[] photo, CancellationToken cancellationToken = default)
    {
    }
}