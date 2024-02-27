using Neural.Onnx.Pipelines;
using Neural.Onnx.Pipelines.Yolo5;
using Neural.Onnx.Pipelines.Yolo8;

namespace PersonDetection.Backend.Application.Services;

public interface INeuralService
{
    public Yolo5ImagePlainPipeline Yolo5ImagePlainPipeline { get; }
    public Yolo5ImageStreamPipeline Yolo5ImageStreamPipeline { get; }

    public Yolo8ImagePlainPipeline Yolo8ImagePlainPipeline { get; }
    public Yolo8ImageStreamPipeline Yolo8ImageStreamPipeline { get; }
}