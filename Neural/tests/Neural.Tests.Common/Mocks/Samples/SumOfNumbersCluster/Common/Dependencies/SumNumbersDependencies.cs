using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Services;

namespace Neural.Tests.Common.Mocks.Samples.SumOfNumbersCluster.Common.Dependencies;

public class SumNumbersDependencies : IDependencyContainer
{
    public readonly SumNumbersService SumNumbersService = new SumNumbersService();
}