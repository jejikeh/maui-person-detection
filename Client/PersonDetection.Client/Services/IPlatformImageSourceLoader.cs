using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Models;

namespace PersonDetection.Client.Services;

public interface IPlatformImageSourceLoader
{
    public ViewPhotoPair LoadViewPhotoPair(Photo originalPhoto, Photo processedPhoto);
}