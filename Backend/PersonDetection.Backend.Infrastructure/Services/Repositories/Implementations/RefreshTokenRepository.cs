using PersonDetection.Backend.Infrastructure.Models.Identity;

namespace PersonDetection.Backend.Infrastructure.Services.Repositories.Implementations;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    public Task<RefreshToken> GetByTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(RefreshToken refreshToken)
    {
        throw new NotImplementedException();
    }
}