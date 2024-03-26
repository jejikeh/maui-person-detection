using Neural.Core.Models;
using Neural.Samples.SumOfNumbersCluster.Services;

namespace Neural.Samples.HelloSumOfNumberPipeline.Common.Dependencies;

public class SumNumbersDependencies : IDependencyContainer
{
    public readonly SumNumbersService SumNumbersService = new SumNumbersService();
}