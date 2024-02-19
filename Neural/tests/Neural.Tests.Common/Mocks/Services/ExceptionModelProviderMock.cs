using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Tests.Common.Mocks.Exceptions;

namespace Neural.Tests.Common.Mocks.Services;

public class ExceptionModelProviderMock : IModelProvider
{
    public const string ExceptionMessage = "I`m a custom model provider, and I`m throwing an exception.";

    public Task<IModel> InitializeAsync<TModel, TModelTask, TWorkerOptions>(IDependencyProvider dependencyProvider,
        TWorkerOptions options) where TModel : class, IModel<TModelTask> where TModelTask : IModelTask
    {
        throw new FakeException(ExceptionMessage);
    }

    public Task<IModel> InitializeAsync<TModel, TModelTask, TOptions, TWorkerOptions>(IDependencyProvider dependencyProvider,
        TOptions modelOptions, TWorkerOptions workerOptions) where TModel : class, IModel<TModelTask, TOptions> where TModelTask : IModelTask where TOptions : IModelOptions
    {
        throw new FakeException(ExceptionMessage);
    }
}