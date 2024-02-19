using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IDependencyProvider
{
    public Task<IDependencyContainer> CreateContainerAsync<T>(T? options = default);
}