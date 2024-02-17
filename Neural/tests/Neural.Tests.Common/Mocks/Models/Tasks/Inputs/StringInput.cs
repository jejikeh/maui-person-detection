using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Models.Tasks.Inputs;

public class StringInput : IModelInput
{
    public string Value { get; set; }
}