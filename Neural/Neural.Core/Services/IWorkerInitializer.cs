namespace Neural.Core.Services;

public interface IWorkerInitializer<out T>
{
    public T Initialize();
}