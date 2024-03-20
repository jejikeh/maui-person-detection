using Neural.Onnx.Models;
using Neural.Tests.Onnx.Mocks.Tasks;

namespace Neural.Tests.Onnx.Mocks.Models;

public class Yolo8ModelMock : OnnxModel<IntToStringTaskMock>
{
    protected override Task<IntToStringTaskMock> ProcessAsync(IntToStringTaskMock task)
    {
        throw new NotImplementedException();
    }
}