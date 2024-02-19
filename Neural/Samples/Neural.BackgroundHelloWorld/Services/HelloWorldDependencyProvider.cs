using Neural.BackgroundHelloWorld.Models;
using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.BackgroundHelloWorld.Services;

public class HelloWorldDependencyProvider : IDependencyProvider
{
    public Task<IDependencyContainer> CreateContainerAsync<T>(T? options = default)
    {
        return Task.FromResult<IDependencyContainer>(new HelloWorldDependencies());
    }
}