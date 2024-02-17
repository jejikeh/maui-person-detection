using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks.Inputs;
using Neural.Tests.Common.Mocks.Models.Tasks.Outputs;

namespace Neural.Tests.Common.Mocks.Models.Tasks;

public class IntToStringTaskMock : IModelTask
{
    public IModelInput Input { get; } = new StringInput();
    public IModelOutput Output { get; set; } = new StringOutput();
}