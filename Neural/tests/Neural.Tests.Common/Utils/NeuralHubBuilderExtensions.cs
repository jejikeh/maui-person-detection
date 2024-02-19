using Neural.Core;
using Neural.Defaults.Common.Options;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;
using Neural.Tests.Common.Mocks.Options;

namespace Neural.Tests.Common.Utils;

public static class NeuralHubBuilderExtensions
{
    public static NeuralHubBuilder AddYolo5Models(this NeuralHubBuilder builder, int count)
    {
        for (var i = 0; i < count; i++)
        {
            builder.AddYolo5Model();
        }
        
        return builder;
    }
    
    public static NeuralHubBuilder AddYolo5Model(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo5ModelMock, StringToStringTaskMock, OnnxOptions>(
                OnnxOptions.FromBuilder(builder, Yolo5Options.ModelPath));
    }

    public static NeuralHubBuilder AddYolo5ModelWithOptions(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo5ModelWithOptionsMock, StringToStringTaskMock, Yolo5Options, OnnxOptions>(
                new Yolo5Options(), 
                OnnxOptions.FromBuilder(builder, Yolo5Options.ModelPath));
    }

    public static NeuralHubBuilder AddYolo8Model(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo8ModelMock, IntToStringTaskMock, OnnxOptions>(
                OnnxOptions.FromBuilder(builder, Yolo8QuantizedOptions.ModelPath));
    }

    public static NeuralHubBuilder AddYolo8ModelWithOptions(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo8ModelWithOptionsMock, StringToStringTaskMock, Yolo8QuantizedOptions, OnnxOptions>(
                new Yolo8QuantizedOptions(),
                OnnxOptions.FromBuilder(builder, Yolo8QuantizedOptions.ModelPath));
    }
}