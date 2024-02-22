using System.Collections.Concurrent;
using Microsoft.ML.OnnxRuntime.Tensors;
using Neural.Onnx.Tasks.ImageToBoxPredictions;
using SixLabors.ImageSharp;

namespace Neural.Onnx.Models.Yolo5.Specifications;

public static class Yolo5OutputSpecification
{
    public static int BatchSize { get; set; } = 0;
    public static int ConfidenceLayer { get; set; } = 4;
    public static int Dimensions { get; set; } = 85;
    
    public static float OverlapThreshold { get; set; } = 0.5f;
    public static float ConfidenceThreshold { get; set; } = 0.5f;
    public static float DimensionValueThreshold { get; set; } = 0.5f;
    
    private const int _xLayer = 0;
    private const int _yLayer = 1;
    private const int _widthLayer = 2;
    private const int _heightLayer = 3;
    
    private const int _labelDimensionOffset = 5;
    
    public static List<Yolo5Prediction> ExtractYoloPredictions(this DenseTensor<float> outputTensor)
    {
        var result = new ConcurrentBag<Yolo5Prediction>();
        
        // We do not calculate Scaling Factors and Paddings, since input image is already scaled

        var outputsEntryCount = (int)outputTensor.Length / Yolo5OutputSpecification.Dimensions;

        Parallel.For(0, outputsEntryCount, prediction =>
        {
            if (outputTensor.IsConfidenceThresholdExceeded(prediction))
            {
                return;
            }

            outputTensor.ScaleTensorLayersAccordingToConfidence(prediction);
            
            outputTensor.ConvertToYoloPredictions(prediction, result.Add);
        });
        
        return result.ToList();
    }

    private static bool IsConfidenceThresholdExceeded(this DenseTensor<float> tensor, int prediction)
    {
        return tensor.GetConfidence(prediction) <= ConfidenceThreshold;
    }

    private static bool IsConfidenceThresholdExceeded(this DenseTensor<float> tensor, int prediction, int dimension)
    {
        return tensor.GetConfidence(prediction, dimension) <= DimensionValueThreshold;
    }

    private static float GetConfidence(this DenseTensor<float> tensor, int prediction)
    {
        return tensor[BatchSize, prediction, ConfidenceLayer];
    }

    private static float GetConfidence(this DenseTensor<float> tensor, int prediction, int dimension)
    {
        return tensor[BatchSize, prediction, dimension];
    }

    private static DenseTensor<float> ScaleTensorLayersAccordingToConfidence(this DenseTensor<float> tensor, int prediction)
    {
        var confidence = tensor.GetConfidence(prediction);
        
        Parallel.For(ConfidenceLayer + 1, Dimensions, dimension =>
        {
            tensor[BatchSize, prediction, dimension] *= confidence;
        });
        
        return tensor;
    }

    private static void ConvertToYoloPredictions(
        this DenseTensor<float> tensor, 
        int prediction,
        Action<Yolo5Prediction> action)
    {
        Parallel.For(5, Dimensions, dimension =>
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
        var xMin = tensor[BatchSize, prediction, _xLayer] - tensor[BatchSize, prediction, _widthLayer] / 2;
        var yMin = tensor[BatchSize, prediction, _yLayer] - tensor[BatchSize, prediction, _heightLayer] / 2;
        
        var xMax = tensor[BatchSize, prediction, _xLayer] + tensor[BatchSize, prediction, _widthLayer] / 2;
        var yMax = tensor[BatchSize, prediction, _yLayer] + tensor[BatchSize, prediction, _heightLayer] / 2;
        
        return new RectangleF(xMin, yMin, xMax - xMin, yMax - yMin);
    }

    private static Yolo5Class ExtractYolo5Class(int dimension)
    {
        return Yolo5Specification.Classes[dimension - _labelDimensionOffset];
    }
}