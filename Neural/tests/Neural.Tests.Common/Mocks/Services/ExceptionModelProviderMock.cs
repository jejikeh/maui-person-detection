using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Tests.Common.Mocks.Exceptions;

namespace Neural.Tests.Common.Mocks.Services;

public class ExceptionModelProviderMock : IModelProvider
{
    public const string ExceptionMessage = "I`m a custom model provider, and I`m throwing an exception.";

    public Task<IModel> InitializeAsync<TModel, TModelTask>(IFileSystemProvider fileSystemProvider, string modelPath) 
        where TModel : class, IModel<TModelTask> 
        where TModelTask : IModelTask
    {
        throw new FakeException(ExceptionMessage);
    }

    public Task<IModel> InitializeAsync<TModel, TModelTask, TOptions>(IFileSystemProvider fileSystemProvider, TOptions modelOptions) 
        where TModel : class, IModel<TModelTask, TOptions> 
        where TOptions : IModelOptions 
        where TModelTask : IModelTask
    {
        throw new FakeException(ExceptionMessage);
    }
}