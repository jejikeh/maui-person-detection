using Neural.Core;
using Neural.Core.Models;
using Neural.Onnx.Common.Dependencies;
using Neural.Onnx.Models.ImageBoxPainter;
using Neural.Onnx.Models.Yolo5;
using Neural.Onnx.Services.Implementations;
using Neural.Onnx.Tasks.BoxPredictionsToImage;
using Neural.Onnx.Tasks.ImageToBoxPredictions;

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

    public static NeuralHubBuilder AddYolo5Model(this NeuralHubBuilder builder, string modelPath)
    {
        return builder.AddOnnxModel<Yolo5Model, ImageToBoxPredictionsTask>(modelPath);
    }
    
    public static NeuralHubBuilder AddYolo5Models(this NeuralHubBuilder builder, string modelPath, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddYolo5Model(modelPath);
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddImageBoxPainterModel(this NeuralHubBuilder builder)
    {
        return builder.AddModel<ImageBoxPainterModel, BoxPredictionsToImageTasks, ImageBoxPainterDependencies>(
            new ImageBoxPainterDependencies(new ImageBoxPainterService()));
    }
    
    public static NeuralHubBuilder AddImageBoxPainterModels(this NeuralHubBuilder builder, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddModel<ImageBoxPainterModel, BoxPredictionsToImageTasks, ImageBoxPainterDependencies>(
                new ImageBoxPainterDependencies(new ImageBoxPainterService()));
        }
        
        return builder;
    }

    public static NeuralHubBuilder AddImageBoxPainterModel(this NeuralHubBuilder builder, ImageBoxPainterDependencies imageBoxPainterDependencies)
    {
        return builder.AddModel<ImageBoxPainterModel, BoxPredictionsToImageTasks, ImageBoxPainterDependencies>(imageBoxPainterDependencies);
    }
}