using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Options;

namespace Neural.Tests.Common.Mocks.Models.Yolo5;

public class Yolo5ModelWithOptionsMock : Yolo5ModelMock, IModel<StringToStringTaskMock, Yolo5Options>
{
    public Yolo5Options? Options { get; set; }
}