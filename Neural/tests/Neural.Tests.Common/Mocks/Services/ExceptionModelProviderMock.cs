using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Tests.Common.Mocks.Exceptions;

namespace Neural.Tests.Common.Mocks.Services;

public class ExceptionModelProviderMock : IModelProvider
{
    public const string ExceptionMessage = "I`m a custom model provider, and I`m throwing an exception.";

    public IModel Initialize<TModel, TModelTask>() 
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        throw new FakeException(ExceptionMessage);
    }

    public IModel Initialize<TModel, TModelTask, TDependencyContainer>(TDependencyContainer dependencyContainer) 
        where TModel : class, IModel<TModelTask, TDependencyContainer> 
        where TModelTask : IModelTask where TDependencyContainer : class, IDependencyContainer
    {
        throw new FakeException(ExceptionMessage);
    }
}