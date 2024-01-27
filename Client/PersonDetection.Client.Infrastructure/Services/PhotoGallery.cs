using PersonDetection.Client.Application.Models;
using PersonDetection.Client.Application.Models.Types;
using PersonDetection.Client.Application.Services;
using PersonDetection.Client.Infrastructure.Common;
using SQLite;

namespace PersonDetection.Client.Infrastructure.Services;

public class PhotoGallery : IPhotoGallery
{
    private readonly SQLiteAsyncConnection _dbConnection;
    private readonly PhotoSaverService _fileSaver;
    
    public PhotoGallery(IInfrastructureConfiguration configuration, PhotoSaverService fileSaver)
    {
        _dbConnection = new SQLiteAsyncConnection(
            configuration.DatabasePath, 
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
        
        _dbConnection.CreateTableAsync<Photo>().Wait();
        _dbConnection.CreateTableAsync<PhotoPair>().Wait();
        
        _fileSaver = fileSaver;
    }
    
    public Task<List<PhotoPair>> GetPhotoPairsAsync()
    {
        return _dbConnection.Table<PhotoPair>().ToListAsync();
    }

    public async Task<Result<PhotoTuple, Error>> GetPhotosAsync(PhotoPair photoPair)
    {
        var original =  await _dbConnection
            .Table<Photo>()
            .Where(x => x.Id == photoPair.OriginalPhotoId)
            .FirstOrDefaultAsync();
        if (original is null)
        {
            return new Error("Original photo not found");
        }
        
        var processed = await _dbConnection
            .Table<Photo>()
            .Where(x => x.Id == photoPair.ProcessedPhotoId)
            .FirstOrDefaultAsync();
        if (processed is null)
        {
            return new Error("Processed photo not found");
        }
        
        return new PhotoTuple
        {
            Original = original,
            Processed = processed
        };
    }

    public async Task<Result<PhotoTuple, Error>> GetPhotosByIdAsync(int id)
    {
        var pair = await _dbConnection.Table<PhotoPair>()
            .FirstOrDefaultAsync(pair => pair.OriginalPhotoId == id);
        if (pair is null)
        {
            return new Error("Photo pair not found");
        }
        
        var original =  await _dbConnection.Table<Photo>()
            .FirstOrDefaultAsync(photo => photo.Id == pair.OriginalPhotoId);
        if (original is null)
        {
            return new Error("Original photo not found");
        }
        
        var processed = await _dbConnection.Table<Photo>()
            .FirstOrDefaultAsync(photo => photo.Id == pair.ProcessedPhotoId);
        if (processed is null)
        {
            return new Error("Processed photo not found");
        }

        return new PhotoTuple
        {
            Original = original,
            Processed = processed
        };
    }

    public async Task AddPairAsync(Photo originalPhoto, Photo processedPhoto)
    {
        await _fileSaver.CachePhotoAsync(processedPhoto);
        await _fileSaver.CachePhotoAsync(originalPhoto);
        
        await _dbConnection.InsertAsync(processedPhoto);
        await _dbConnection.InsertAsync(originalPhoto);

        await _dbConnection.InsertAsync(new PhotoPair
        {
            OriginalPhotoId = originalPhoto.Id,
            ProcessedPhotoId = processedPhoto.Id
        });
    }

    public async Task<Result<string, Error>> DeletePairAsync(Photo photoFromPair)
    {
        var pair = await _dbConnection.Table<PhotoPair>()
            .FirstOrDefaultAsync(photoPair => photoPair.OriginalPhotoId == photoFromPair.Id);
        if (pair is null)
        {
            return new Error("Photo pair not found");
        }
        
        await _dbConnection.DeleteAsync(pair);
        await _dbConnection.Table<Photo>().DeleteAsync(photo => photo.Id == pair.OriginalPhotoId);
        
        return $"Photo pair {pair.OriginalPhotoId} and {pair.ProcessedPhotoId} were deleted";
    }
}