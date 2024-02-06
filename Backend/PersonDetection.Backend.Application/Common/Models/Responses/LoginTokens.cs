namespace PersonDetection.Backend.Application.Common.Models.Responses;

public record LoginTokens(
    string RefreshToken,
    string JwtToken,
    string UserName,
    string Email,
    Guid Id);
