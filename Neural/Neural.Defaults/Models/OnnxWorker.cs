using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;

namespace Neural.Defaults.Models;

public class OnnxWorker(MemoryStream memoryStream) : IModelWorker
{
    private readonly InferenceSession _inferenceSession = new InferenceSession(memoryStream.ToArray(), new SessionOptions());
    
    public Task<IModelTask> RunAsync(IModelTask input)
    {
        throw new NotImplementedException();
    }

    public T CastToWorker<T>() where T : class
    {
        throw new NotImplementedException();
    }
}