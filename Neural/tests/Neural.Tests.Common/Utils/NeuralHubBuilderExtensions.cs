using Neural.Core;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Models.Yolo8;
using Neural.Tests.Common.Mocks.Options;

namespace Neural.Tests.Common.Utils;

public static class NeuralHubBuilderExtensions
{
    public static NeuralHubBuilder AddYolo5Model(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo5ModelMock, StringToStringTaskMock>(Yolo5Options.ModelPath);
    }

    public static NeuralHubBuilder AddYolo5ModelWithOptions(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo5ModelWithOptionsMock, StringToStringTaskMock, Yolo5Options>(new Yolo5Options());
    }

    public static NeuralHubBuilder AddYolo8Model(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo8ModelMock, IntToStringTaskMock>(Yolo8QuantizedOptions.ModelPath);
    }

    public static NeuralHubBuilder AddYolo8ModelWithOptions(this NeuralHubBuilder builder)
    {
        return builder
            .AddModel<Yolo8ModelWithOptionsMock, StringToStringTaskMock, Yolo8QuantizedOptions>(new Yolo8QuantizedOptions());
    }
}