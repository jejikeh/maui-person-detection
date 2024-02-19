using Neural.Core.Models;

namespace Neural.Core.Services;

public interface IModelWorkerProvider
{
    public Task<IModelWorker> CreateWorkerAsync<T>(T? options = default);
}