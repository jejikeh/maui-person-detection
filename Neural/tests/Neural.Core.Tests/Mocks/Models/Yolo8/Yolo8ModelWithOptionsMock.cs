using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Core.Tests.Mocks.Options;

namespace Neural.Core.Tests.Mocks.Models.Yolo8;

public class Yolo8ModelWithOptionsMock : IModel<Yolo8QuantizedOptions>
{
    public InferenceSession? InferenceSession { get; set; }
    public Yolo8QuantizedOptions? Options { get; set; }
}