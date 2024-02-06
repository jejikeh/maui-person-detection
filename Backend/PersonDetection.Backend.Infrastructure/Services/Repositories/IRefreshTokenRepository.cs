using PersonDetection.Backend.Infrastructure.Models.Identity;

namespace PersonDetection.Backend.Infrastructure.Services.Repositories;

public interface IRefreshTokenRepository
{
    public Task<RefreshToken> GetByTokenAsync(string token);
    public Task AddAsync(RefreshToken refreshToken);
}