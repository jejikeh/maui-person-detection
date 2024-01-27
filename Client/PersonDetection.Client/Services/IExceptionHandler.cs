using System.Runtime.ExceptionServices;

namespace PersonDetection.Client.Services;

public interface IExceptionHandler
{
    void OnException(object sender, FirstChanceExceptionEventArgs firstChanceExceptionEventArgs);
}