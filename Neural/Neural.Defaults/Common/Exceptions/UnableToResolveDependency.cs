namespace Neural.Defaults.Common.Exceptions;

public class UnableToResolveDependency<TFrom, TTo>() 
    : Exception($"Cannot cast {typeof(TFrom).Name} to {typeof(TTo).Name}. Please check your configuration.")
{
    
}