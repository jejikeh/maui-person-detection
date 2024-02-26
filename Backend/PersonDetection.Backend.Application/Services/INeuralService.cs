using Neural.Onnx.Pipelines;

namespace PersonDetection.Backend.Application.Services;

public interface INeuralService
{
    public Yolo5ImagePlainPipeline Yolo5ImagePlainPipeline { get; }
    public Yolo5ImageStreamPipeline Yolo5ImageStreamPipeline { get; } 
}