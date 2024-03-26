using Neural.Core.Models;
using Neural.Onnx.Services;

namespace Neural.Onnx.Common.Dependencies;

public class ImageBoxPainterDependencies(IImageBoxPainterService _imageBoxPainterService) : IDependencyContainer
{
    public IImageBoxPainterService ImageBoxPainterService => _imageBoxPainterService;
}