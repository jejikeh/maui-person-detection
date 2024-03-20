using Neural.Core.Models;
using Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Services;

namespace Neural.Tests.Common.Mocks.Samples.HelloNumberCluster.Common.Dependencies;

public class HelloNumberDependencies : IDependencyContainer
{
    public readonly HelloNumberService HelloNumberService = new HelloNumberService();
}