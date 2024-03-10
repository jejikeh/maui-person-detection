using CommunityToolkit.HighPerformance.Helpers;
using Minio;
using Minio.DataModel.Args;

namespace PersonDetection.Backend.Infrastructure.Services.Implementations;

public class ImageBucketService(IMinioClient _minioClient) : IImageBucketService
{
    private const string _bucketName = "person-detection";
    
    public async Task SavePhotoAsync(string photoName, string photo)
    {
        var bytes = Convert.FromBase64String(photo);
        
        using var photoMemoryStream = new MemoryStream(bytes);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
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
            .WithBucket(_bucketName)
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
}