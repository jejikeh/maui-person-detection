using Microsoft.ML.OnnxRuntime;

namespace Neural.Core.Models;

public interface IModel
{
    public InferenceSession? InferenceSession { get; set; }

    public void Initialize(MemoryStream memoryStream)
    {
        InferenceSession = new InferenceSession(memoryStream.ToArray(), new SessionOptions());
    }
}

public interface IModel<TOptions> : IModel where TOptions : IModelOptions
{
    public TOptions? Options { get; set; }
    
    public void Initialize(MemoryStream memoryStream, TOptions options)
    {
        Initialize(memoryStream);
        Options = options;
    }
}