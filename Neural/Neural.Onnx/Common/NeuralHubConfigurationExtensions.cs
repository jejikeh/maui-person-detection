using Neural.Core;
using Neural.Core.Models;
using Neural.Onnx.Common.Dependencies;

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
}