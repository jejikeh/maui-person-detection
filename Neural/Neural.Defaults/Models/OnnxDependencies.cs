using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;

namespace Neural.Defaults.Models;

public class OnnxDependencies(MemoryStream memoryStream) : IDependencyContainer
{
    private readonly InferenceSession _inferenceSession = new InferenceSession(memoryStream.ToArray(), new SessionOptions());
    
    public Task<IModelTask> RunAsync(IModelTask input)
    {
        throw new NotImplementedException();
    }

    public T CastToDependency<T>() where T : class
    {
        throw new NotImplementedException();
    }
}