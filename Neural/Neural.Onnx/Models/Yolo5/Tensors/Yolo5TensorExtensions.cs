using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Models.Yolo5.Specifications;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo5.Tensors;

public static class Yolo5TensorExtensions
{
    public static void ConvertToYoloPredictions(this DenseTensor<float> tensor, int prediction, Action<Yolo5Prediction> action)
    {
        Parallel.For(5, Yolo5OutputSpecification.Dimensions, dimension =>
        {
            if (tensor.IsConfidenceThresholdExceeded(prediction, dimension))
            {
                return;
            }

            var boundingBox = tensor.ExtractBoundingBox(prediction);
            var yolo5Class = ExtractYolo5Class(dimension);
            
            action(new Yolo5Prediction(
                yolo5Class,
                tensor.GetConfidence(prediction, dimension),
                boundingBox));
        });
    }

    private static RectangleF ExtractBoundingBox(this Tensor<float> tensor, int prediction)
    {
        var xMin = tensor.GetMinDimensionValue(
            prediction, 
            Yolo5OutputSpecification.XLayer, 
            Yolo5OutputSpecification.WidthLayer);
        
        var yMin = tensor.GetMinDimensionValue(
            prediction, 
            Yolo5OutputSpecification.YLayer, 
            Yolo5OutputSpecification.HeightLayer);
        
        var xMax = tensor.GetMaxDimensionValue(
            prediction, 
            Yolo5OutputSpecification.XLayer, 
            Yolo5OutputSpecification.WidthLayer);
        
        var yMax = tensor.GetMaxDimensionValue(
            prediction, 
            Yolo5OutputSpecification.YLayer, 
            Yolo5OutputSpecification.HeightLayer);
        
        return new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin);
    }
    
    private static float GetMaxDimensionValue(this Tensor<float> tensor, int prediction, int layer1, int layer2)
    {
        var layer1Value = tensor.ExtractLayerPredictionValue(prediction, layer1);
        var layer2Value = tensor.ExtractLayerPredictionValue(prediction, layer2) / 2;
        
        return layer1Value + layer2Value;
    }
    
    private static float GetMinDimensionValue(this Tensor<float> tensor, int prediction, int layer1, int layer2)
    {
        var layer1Value = tensor.ExtractLayerPredictionValue(prediction, layer1);
        var layer2Value = tensor.ExtractLayerPredictionValue(prediction, layer2) / 2;
        
        return layer1Value - layer2Value;
    }
    
    private static float ExtractLayerPredictionValue(this Tensor<float> tensor, int prediction, int layer)
    {
        return tensor[Yolo5OutputSpecification.BatchSize, prediction, layer];
    }

    private static YoloClass ExtractYolo5Class(int dimension)
    {
        return Yolo5Specification.Classes[dimension - Yolo5OutputSpecification.LabelDimensionOffset];
    }
    
    public static DenseTensor<float> ScaleTensorLayersAccordingToConfidence(this DenseTensor<float> tensor, int prediction)
    {
        var confidence = tensor.GetConfidence(prediction);
        
        Parallel.For(
            Yolo5OutputSpecification.ConfidenceLayer + 1, 
            Yolo5OutputSpecification.Dimensions, 
            dimension =>
        {
            tensor[Yolo5OutputSpecification.BatchSize, prediction, dimension] *= confidence;
        });
        
        return tensor;
    }
    
    public static bool IsConfidenceThresholdExceeded(this DenseTensor<float> tensor, int prediction)
    {
        return tensor.GetConfidence(prediction) <= Yolo5OutputSpecification.ConfidenceThreshold;
    }

    private static bool IsConfidenceThresholdExceeded(this DenseTensor<float> tensor, int prediction, int dimension)
    {
        return tensor.GetConfidence(prediction, dimension) <= Yolo5OutputSpecification.DimensionValueThreshold;
    }

    private static float GetConfidence(this DenseTensor<float> tensor, int prediction)
    {
        return tensor[Yolo5OutputSpecification.BatchSize, prediction, Yolo5OutputSpecification.ConfidenceLayer];
    }

    private static float GetConfidence(this DenseTensor<float> tensor, int prediction, int dimension)
    {
        return tensor[Yolo5OutputSpecification.BatchSize, prediction, dimension];
    }
}