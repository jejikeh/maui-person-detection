using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Models.Tasks;
using Neural.Tests.Common.Mocks.Models.Yolo5;
using Neural.Tests.Common.Mocks.Options;

namespace Neural.Tests.Common.Mocks.Models.Yolo8;

public class Yolo8ModelWithOptionsMock : Yolo5ModelMock, IModel<StringToStringTaskMock, Yolo8QuantizedOptions>
{
    public Yolo8QuantizedOptions? Options { get; set; }
}