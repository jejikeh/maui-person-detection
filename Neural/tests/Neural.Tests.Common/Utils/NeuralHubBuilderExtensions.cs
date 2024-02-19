using Neural.Core;
using Neural.Defaults.Common.Dependencies;
using Neural.Tests.Common.Mocks;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;

namespace Neural.Tests.Common.Utils;

public static class NeuralHubBuilderExtensions
{
    public static NeuralHubBuilder AddYolo5ModelsStringToString(this NeuralHubBuilder builder, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddYolo5ModelStringToString();
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddYolo5ModelStringToString(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo5ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
                OnnxDependencies.FromBuilder(builder, Constants.Yolo5ModelPath));
    }

    public static NeuralHubBuilder AddYolo8ModelIntToString(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo8ModelIntToStringMock, IntToStringTaskMock, OnnxDependencies>(
                OnnxDependencies.FromBuilder(builder, Constants.Yolo8ModelPath));
    }

    public static NeuralHubBuilder AddYolo8ModelStringToString(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo8ModelStringToStringMock, StringToStringTaskMock, OnnxDependencies>(
                OnnxDependencies.FromBuilder(builder, Constants.Yolo8ModelPath));
    }
}