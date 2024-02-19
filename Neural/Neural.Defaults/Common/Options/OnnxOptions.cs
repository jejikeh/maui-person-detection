using Neural.Core;
using Neural.Core.Services;

namespace Neural.Defaults.Common.Options;

public class OnnxOptions(IFileSystemProvider _fileSystemProvider, string _modelPath)
{
    public IFileSystemProvider FileSystemProvider { get; set; } = _fileSystemProvider;
    public string ModelPath { get; set; } = _modelPath;

    public static OnnxOptions FromBuilder(NeuralHubBuilder builder, string modelPath)
    {
        return new OnnxOptions(builder.FileSystemProvider, modelPath);
    }
}