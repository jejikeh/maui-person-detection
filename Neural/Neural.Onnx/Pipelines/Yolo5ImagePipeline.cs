using Neural.Core;
using Neural.Defaults.Models;
using Neural.Onnx.Clusters;
using Neural.Onnx.Models.ImageBoxPainter;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;

namespace Neural.Onnx.Pipelines;

public abstract class Yolo5ImagePipeline : IPipeline
{
    protected Yolo5Cluster? Yolo5Cluster;
    protected Cluster<ImageBoxPainterModel, BoxPredictionsToImageTasks>? ImageBoxPainterCluster;
    
    public bool Init(NeuralHub neuralHub)
    {
        Yolo5Cluster = neuralHub.ShapeCluster<Yolo5Cluster>();
        ImageBoxPainterCluster = neuralHub.ShapeCluster<Cluster<ImageBoxPainterModel, BoxPredictionsToImageTasks>>();
        
        return ClustersInitialized();
    }

    protected bool ClustersInitialized()
    {
        return Yolo5Cluster is not null && ImageBoxPainterCluster is not null;
    }
}