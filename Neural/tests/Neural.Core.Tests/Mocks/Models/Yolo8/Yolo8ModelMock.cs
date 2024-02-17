using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Models.Yolo8;

public class Yolo8ModelMock : IModel
{
    public InferenceSession? InferenceSession { get; set; }
}