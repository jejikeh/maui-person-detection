using SQLite;

namespace PersonDetection.Client.Application.Models;

[Table("Photos")]
public class Photo
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    
    public static implicit operator Photo(string base64) => new() { Content = base64 };
}