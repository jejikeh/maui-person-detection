using Neural.Core;
using Neural.Defaults.Models;
using Neural.Onnx.Clusters;
using Neural.Onnx.Models.ImageBoxPainter;
using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;

namespace Neural.Onnx.Pipelines.Yolo8;

public abstract class Yolo8ImagePipeline : IPipeline
{
    protected Yolo8Cluster? Yolo8Cluster;
    protected Cluster<ImageSegmentationPainterModel, SegmentationPredictionsToImageTask>? ImageBoxPainterCluster;
    
    public bool Init(NeuralHub neuralHub)
    {
        Yolo8Cluster = neuralHub.ShapeCluster<Yolo8Cluster>();
        ImageBoxPainterCluster = neuralHub.ShapeCluster<Cluster<ImageSegmentationPainterModel, SegmentationPredictionsToImageTask>>();
        
        return ClustersInitialized();
    }

    protected bool ClustersInitialized()
    {
        return Yolo8Cluster is not null && ImageBoxPainterCluster is not null;
    }
}