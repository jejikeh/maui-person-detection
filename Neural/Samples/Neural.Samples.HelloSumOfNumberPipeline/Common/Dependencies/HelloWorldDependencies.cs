using Neural.Core.Models;
using Neural.Samples.HelloSumOfNumberPipeline.Services;

namespace Neural.Samples.HelloSumOfNumberPipeline.Common.Dependencies;

public class HelloWorldDependencies : IDependencyContainer
{
    public readonly HelloNumberService HelloNumberService = new HelloNumberService();
}