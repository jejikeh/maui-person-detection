using Neural.Onnx.Models;
using Neural.Tests.Onnx.Mocks.Tasks;

namespace Neural.Tests.Onnx.Mocks.Models;

public class Yolo5ModelMock : OnnxModel<IntToStringTaskMock>
{
    public override Task<IntToStringTaskMock> RunAsync(IntToStringTaskMock task)
    {
        throw new NotImplementedException();
    }
}