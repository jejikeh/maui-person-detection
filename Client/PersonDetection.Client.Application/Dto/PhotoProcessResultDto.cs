using PersonDetection.Client.Application.Models;

namespace PersonDetection.Client.Application.Dto;

public class PhotoProcessResultDto
{
    public Guid Id { get; set; }
    public string Photo { get; set; } = string.Empty;
}