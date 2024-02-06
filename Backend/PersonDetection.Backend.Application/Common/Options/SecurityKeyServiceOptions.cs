namespace PersonDetection.Backend.Application.Common.Options;

public class SecurityKeyServiceOptions
{
    public string RsaPath { get; set; } = string.Empty;
    public bool GenerateNewKey { get; set; } = true;
}