namespace PersonDetection.Client.Common.Services;

public interface IPhotoProcessService
{
    public Task<byte[]> ProcessPhoto(byte[] photo, CancellationToken cancellationToken = default);
}