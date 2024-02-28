using Neural.Core;
using Neural.Onnx.Pipelines.Yolo5;
using Neural.Onnx.Pipelines.Yolo8;

namespace PersonDetection.Backend.Application.Services.Implementations;

public class OnnxNeuralService(NeuralHub _neuralHub) : IOnnxNeuralService
{
    public Yolo5ImagePlainPipeline Yolo5ImagePlainPipeline { get; } =
        _neuralHub.ExtractPipeline<Yolo5ImagePlainPipeline>()!;
    
    public Yolo5ImageStreamPipeline Yolo5ImageStreamPipeline { get; } =
        _neuralHub.ExtractPipeline<Yolo5ImageStreamPipeline>()!;

    public Yolo8ImagePlainPipeline Yolo8ImagePlainPipeline { get; } =
        _neuralHub.ExtractPipeline<Yolo8ImagePlainPipeline>()!;

    public Yolo8ImageStreamPipeline Yolo8ImageStreamPipeline { get; } =
        _neuralHub.ExtractPipeline<Yolo8ImageStreamPipeline>()!;
}