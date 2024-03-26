using Neural.Core.Models;

namespace Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Tasks.IntToString;

public class IntInput(int _input) : IModelInput
{
    public int Value => _input;
}