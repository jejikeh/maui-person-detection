using Microsoft.VisualBasic;
using Neural.Core;
using Neural.Core.Models;
using Neural.Onnx.Common.Dependencies;
using Neural.Onnx.Models.Yolo5;
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
        return builder.AddOnnxModel<Yolo5Model, ImageToBoxPredictionsYolo5Task>(modelPath);
    }
    
    public static NeuralHubBuilder AddYolo5Models(this NeuralHubBuilder builder, string modelPath, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddYolo5Model(modelPath);
        }
        
        return builder;
    }
}