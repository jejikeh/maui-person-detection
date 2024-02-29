using Neural.Core.Models;
using Neural.Samples.SumOfNumbersCluster.Services;

namespace Neural.Samples.SumOfNumbersCluster.Common.Dependencies;

public class SumNumbersDependencies : IDependencyContainer
{
    public readonly SumNumbersService SumNumbersService = new SumNumbersService();
    public readonly ModelNameProvider ModelNameProvider = new ModelNameProvider();
}