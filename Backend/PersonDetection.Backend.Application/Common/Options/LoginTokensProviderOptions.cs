namespace PersonDetection.Backend.Application.Common.Options;

public class LoginTokensProviderOptions
{
    public int RefreshTokenTtl { get; set; }
    public int ContentLength { get; set; }
    public string Host { get; set; } = string.Empty;
}