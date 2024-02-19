using SQLite;

namespace PersonDetection.Client.Application.Models;

[Table("PhotoPair")]
public class PhotoPair
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    
    [Indexed]
    public int OriginalPhotoId { get; set; }
    
    [Indexed]
    public int ProcessedPhotoId { get; set; }
}