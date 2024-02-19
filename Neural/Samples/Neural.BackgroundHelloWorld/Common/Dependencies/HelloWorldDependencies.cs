using Neural.BackgroundHelloWorld.Services;
using Neural.Core.Models;

namespace Neural.BackgroundHelloWorld.Common.Dependencies;

public class HelloWorldDependencies : IDependencyContainer
{
    public readonly HelloWorldService HelloWorldService = new HelloWorldService();
    public readonly ModelNameProvider ModelNameProvider = new ModelNameProvider();
}