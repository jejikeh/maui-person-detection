namespace Neural.Core.Models;

public interface IModelWorker
{
    public T CastToWorker<T>() where T : class;
}