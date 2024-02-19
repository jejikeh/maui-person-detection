using Neural.BackgroundHelloWorld.Services;
using Neural.Core.Models;
using Neural.Defaults.Common.Exceptions;

namespace Neural.BackgroundHelloWorld.Models;

public class HelloWorldWorker : IModelWorker
{
    private readonly HelloWorldService _helloWorldService = new HelloWorldService();
    
    public T CastToWorker<T>() where T : class
    {
        if (typeof(T) == typeof(HelloWorldService))
        {
            return _helloWorldService as T ?? throw new UnableToResolveDependency<T, HelloWorldService>();
        }
        
        throw new InvalidOperationException($"Cannot cast {typeof(T)} to {typeof(HelloWorldService)}");
    }
}