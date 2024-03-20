using Neural.Core;
using Neural.Core.Models;
using Neural.Onnx.Common.Dependencies;
using Neural.Onnx.Common.Options;
using Neural.Onnx.Models.ImageBoxPainter;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Models.Yolo5.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Models.Yolo5.Tasks.ImageToBoxPredictions;
using Neural.Onnx.Models.Yolo8;
using Neural.Onnx.Models.Yolo8.SegmentationsPredictionsToImage;
using Neural.Onnx.Services.Implementations;
using Neural.Onnx.Tasks.ImageToSegmentation;

namespace Neural.Onnx.Common;

public static class NeuralHubConfigurationExtensions
{
    public static NeuralHubBuilder AddOnnxModels<TModel, TModelTask>(this NeuralHubBuilder builder, string modelPath, int count)
        where TModel : class, IModel<TModelTask, OnnxDependencies> 
        where TModelTask : IModelTask
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddModel<TModel, TModelTask, OnnxDependencies>(OnnxDependencies.FromBuilder(builder, modelPath));
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddOnnxModel<TModel, TModelTask>(this NeuralHubBuilder builder, string modelPath)
        where TModel : class, IModel<TModelTask, OnnxDependencies> 
        where TModelTask : IModelTask
    {
        return builder
            .AddModel<TModel, TModelTask, OnnxDependencies>(OnnxDependencies.FromBuilder(builder, modelPath));
    }
    
    public static NeuralHubBuilder AddOnnxModel<TModel, TModelTask>(this NeuralHubBuilder builder, byte[] modelBytes)
        where TModel : class, IModel<TModelTask, OnnxDependencies> 
        where TModelTask : IModelTask
    {
        return builder
            .AddModel<TModel, TModelTask, OnnxDependencies>(OnnxDependencies.FromBytes(modelBytes));
    }

    public static NeuralHubBuilder AddYolo5Model(this NeuralHubBuilder builder, string modelPath)
    {
        return builder.AddOnnxModel<Yolo5Model, ImageToBoxPredictionsTask>(modelPath);
    }
    
    public static NeuralHubBuilder AddYolo5Model(this NeuralHubBuilder builder, byte[] modelBytes)
    {
        return builder.AddOnnxModel<Yolo5Model, ImageToBoxPredictionsTask>(modelBytes);
    }
    
    public static NeuralHubBuilder AddYolo8Model(this NeuralHubBuilder builder, string modelPath)
    {
        return builder.AddOnnxModel<Yolo8Model, ImageToSegmentationTask>(modelPath);
    }
    
    public static NeuralHubBuilder AddYolo5Models(this NeuralHubBuilder builder, string modelPath, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddYolo5Model(modelPath);
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddYolo5Models(this NeuralHubBuilder builder, byte[] modelBytes, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddYolo5Model(modelBytes);
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddYolo8Models(this NeuralHubBuilder builder, string modelPath, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddYolo8Model(modelPath);
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddImageBoxPainterModels(
        this NeuralHubBuilder builder, 
        ImageBoxPainterOptions options, 
        int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddModel<ImageBoxPainterModel, BoxPredictionsToImageTask, ImageBoxPainterDependencies>(
                new ImageBoxPainterDependencies(new ImageBoxPainterService(options)));
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddImageSegmentationPainterModels(this NeuralHubBuilder builder, ImageBoxPainterOptions options, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddModel<ImageSegmentationPainterModel, SegmentationPredictionsToImageTask, ImageBoxPainterDependencies>(
                new ImageBoxPainterDependencies(new ImageBoxPainterService(options)));
        }
        
        return builder;
    }
}