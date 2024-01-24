using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;
using SQLite;

namespace PersonDetection.Client.Infrastructure.Services;

public class ProcessedPhotoGallery : IProcessedPhotoGallery
{
    private readonly SQLiteAsyncConnection _dbConnection;
    
    public ProcessedPhotoGallery(IInfrastructureConfiguration configuration)
    {
        _dbConnection = new SQLiteAsyncConnection(configuration.DatabasePath, 
            SQLiteOpenFlags.ReadWrite | 
            SQLiteOpenFlags.Create | 
            SQLiteOpenFlags.SharedCache);
        
        _dbConnection.CreateTableAsync<ProcessedPhoto>().Wait();
    }
    
    public Task<List<ProcessedPhoto>> GetAsync()
    {
        return _dbConnection.Table<ProcessedPhoto>().ToListAsync();
    }

    public Task AddAsync(ProcessedPhoto processedPhoto)
    {
        return _dbConnection.InsertAsync(processedPhoto);
    }

    public Task DeleteAsync(ProcessedPhoto processedPhoto)
    {
        return _dbConnection.DeleteAsync<ProcessedPhoto>(processedPhoto);
    }
}