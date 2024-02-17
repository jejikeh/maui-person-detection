using Moq;
using Neural.Core.Models;
using Neural.Core.Services;
using Neural.Core.Tests.Mocks.Exceptions;

namespace Neural.Core.Tests.Mocks.Services;

public class ExceptionModelProviderMock : IModelProvider
{
    public const string ExceptionMessage = "I`m a custom model provider, and I`m throwing an exception.";

    public Task<IModel> InitializeAsync<TModel>(IFileSystemProvider fileSystemProvider, string modelPath) where TModel : class, IModel
    {
        throw new FakeException(ExceptionMessage);
    }

    public Task<IModel> InitializeAsync<TModel, TOptions>(IFileSystemProvider fileSystemProvider, TOptions modelOptions) where TModel : class, IModel<TOptions> where TOptions : IModelOptions
    {
        throw new FakeException(ExceptionMessage);
    }
}