using Microsoft.ML.OnnxRuntime;
using Neural.Core;
using Neural.Core.Models;
using Neural.Core.Services;

namespace Neural.Onnx.Common.Dependencies;

public class OnnxDependencies(InferenceSession _inferenceSession) : IDependencyContainer
{
    public InferenceSession InferenceSession { get; set; } = _inferenceSession;

    public static OnnxDependencies FromBuilder(NeuralHubBuilder builder, string modelPath)
    {
        return FromBuilderAsync(builder, modelPath).Result;
    }
    
    public static async Task<OnnxDependencies> FromBuilderAsync(NeuralHubBuilder builder, string modelPath)
    {
        var inferenceSession = await InitializeInferenceSessionAsync(
            builder.FileSystemProvider,
            modelPath);
        
        return new OnnxDependencies(inferenceSession);
    }
    
    private static async Task<InferenceSession> InitializeInferenceSessionAsync(
        IFileSystemProvider fileSystemProvider, string modelPath)
    {
        using var modelStream = new MemoryStream();
        
        var stream = await fileSystemProvider.GetFileStreamAsync(modelPath);
        
        await stream.CopyToAsync(modelStream);

        return new InferenceSession(modelStream.ToArray(), new SessionOptions());
    }
}