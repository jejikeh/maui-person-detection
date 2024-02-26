using Neural.Core;
using Neural.Onnx.Pipelines;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class NeuralService(NeuralHub _neuralHub) : INeuralService
{
    public Yolo5ImagePlainPipeline Yolo5ImagePlainPipeline { get; } =
        _neuralHub.ExtractPipeline<Yolo5ImagePlainPipeline>()!;
    
    public Yolo5ImageStreamPipeline Yolo5ImageStreamPipeline { get; } =
        _neuralHub.ExtractPipeline<Yolo5ImageStreamPipeline>()!;
}