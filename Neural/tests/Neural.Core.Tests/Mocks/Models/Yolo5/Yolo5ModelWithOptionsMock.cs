using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;
using Neural.Core.Tests.Mocks.Options;

namespace Neural.Core.Tests.Mocks.Models.Yolo5;

public class Yolo5ModelWithOptionsMock : IModel<Yolo5Options>
{
    public InferenceSession? InferenceSession { get; set; }
    public Yolo5Options? Options { get; set; }
}