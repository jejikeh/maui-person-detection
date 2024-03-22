using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using PersonDetection.Backend.Infrastructure.Common.Options;

namespace PersonDetection.Backend.Infrastructure.Services.Implementations;

public class ImageBucketService(IMinioClient _minioClient, IOptions<ImageBucketOptions> options) : IImageBucketService
{
    public async Task SavePhotoAsync(string photoName, string photo)
    {
        await CreateBucketIfNotExistsAsync();

        var bytes = Convert.FromBase64String(photo);

        using var photoMemoryStream = new MemoryStream(bytes);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(options.Value.BucketName)
            .WithStreamData(photoMemoryStream)
            .WithObject(photoName)
            .WithContentType("image/png")
            .WithObjectSize(photoMemoryStream.Length);

        await _minioClient.PutObjectAsync(putObjectArgs);
    }

    public async Task<string> GetPhotoAsync(string photoName)
    {
        var photo = string.Empty;

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(options.Value.BucketName)
            .WithObject(photoName)
            .WithCallbackStream((stream) =>
            {
                using var memoryStream = new MemoryStream();

                stream.CopyTo(memoryStream);

                photo = Convert.ToBase64String(memoryStream.ToArray());
            });

        await _minioClient.GetObjectAsync(getObjectArgs);

        return photo;
    }

    private async Task CreateBucketIfNotExistsAsync()
    {
        var exists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs()
            .WithBucket(options.Value.BucketName));

        if (exists)
        {
            return;
        }

        var makeBucketArgs = new MakeBucketArgs()
            .WithBucket(options.Value.BucketName);

        await _minioClient.MakeBucketAsync(makeBucketArgs);
    }
}