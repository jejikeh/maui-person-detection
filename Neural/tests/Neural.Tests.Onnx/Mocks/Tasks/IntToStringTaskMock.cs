using Neural.Core.Models;

namespace Neural.Tests.Onnx.Mocks.Tasks;

public class IntToStringTaskMock : IModelTask
{
    public IModelInput Input { get; set; }
    public IModelOutput Output { get; set; }
    public void SetOutput(IModel model, object value)
    {
        throw new NotImplementedException();
    }
}