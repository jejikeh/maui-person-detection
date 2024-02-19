using Neural.BackgroundHelloWorld.Models;
using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.BackgroundHelloWorld.Services;

public class HelloWorldWorkerProvider : IModelWorkerProvider
{
    public Task<IModelWorker> CreateWorkerAsync<T>(T? options = default)
    {
        return Task.FromResult<IModelWorker>(new HelloWorldWorker());
    }
}