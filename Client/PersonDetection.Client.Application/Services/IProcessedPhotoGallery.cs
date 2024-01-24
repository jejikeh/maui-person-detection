using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Application.Services;

public interface IProcessedPhotoGallery
{
    public Task<List<ProcessedPhoto>> GetAsync();
    public Task AddAsync(ProcessedPhoto processedPhoto);
    public Task DeleteAsync(ProcessedPhoto photoBase64);
}