using Neural.Core.Models;
using Neural.Samples.BackgroundHelloWorld.Services;

namespace Neural.Samples.BackgroundHelloWorld.Common.Dependencies;

public class HelloWorldDependencies : IDependencyContainer
{
    public readonly HelloWorldService HelloWorldService = new HelloWorldService();
    public readonly ModelNameProvider ModelNameProvider = new ModelNameProvider();
}