using Microsoft.ML.OnnxRuntime;
using Neural.Core.Models;

namespace Neural.Core.Tests.Mocks.Models.Yolo5;

public class Yolo5ModelMock : IModel
{
    public InferenceSession? InferenceSession { get; set; }
}