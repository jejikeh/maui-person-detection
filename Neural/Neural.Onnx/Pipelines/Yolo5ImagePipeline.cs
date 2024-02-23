using Neural.Core;
using Neural.Defaults.Models;
using Neural.Onnx.Models.ImageBoxPainter;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Tasks.ImageToBoxPredictions;

namespace Neural.Onnx.Pipelines;

public abstract class Yolo5ImagePipeline : IPipeline
{
    protected Cluster<Yolo5Model, ImageToBoxPredictionsTask>? Yolo5Cluster;
    protected Cluster<ImageBoxPainterModel, BoxPredictionsToImageTasks>? ImageBoxPainterCluster;
    
    public bool Init(NeuralHub neuralHub)
    {
        Yolo5Cluster = neuralHub.ShapeCluster<Cluster<Yolo5Model, ImageToBoxPredictionsTask>>();
        ImageBoxPainterCluster = neuralHub.ShapeCluster<Cluster<ImageBoxPainterModel, BoxPredictionsToImageTasks>>();
        
        return ClustersInitialized();
    }

    protected bool ClustersInitialized()
    {
        return Yolo5Cluster is not null && ImageBoxPainterCluster is not null;
    }
}