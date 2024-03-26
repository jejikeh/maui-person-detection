using FluentAssertions;
using Neural.Core.Models;

namespace Neural.Tests.Common.Utils;

public static class AssertionUtils
{
    public static void ShouldHaveState<TModel>(this IEnumerable<TModel> models, ModelStatus state) where TModel : IModel
    {
        models.Should().Match(modelsEnumerable => 
            modelsEnumerable.All(yolo5ModelMock => yolo5ModelMock.Status == state));
    }
}