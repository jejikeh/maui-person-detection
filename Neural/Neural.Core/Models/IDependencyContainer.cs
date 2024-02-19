namespace Neural.Core.Models;

public interface IDependencyContainer
{
    public T CastToDependency<T>() where T : class;
}