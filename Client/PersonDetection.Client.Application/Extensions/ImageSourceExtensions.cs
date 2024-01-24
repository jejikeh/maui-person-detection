namespace PersonDetection.Client.Application.Extensions;

public static class ImageSourceExtensions
{
    public static string ToBase64(this Stream imageStream)
    {
        using var memoryStream = new MemoryStream();
        imageStream.CopyTo(memoryStream);
        return Convert.ToBase64String(memoryStream.ToArray());
    }
}