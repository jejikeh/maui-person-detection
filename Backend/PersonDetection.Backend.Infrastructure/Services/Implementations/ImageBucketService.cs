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
        
        var response = await _minioClient.PutObjectAsync(putObjectArgs);
        
        Console.WriteLine(response);
    }

    public Task<List<string>> GetPhotosAsync(int page, int count)
    {
        
    }
}