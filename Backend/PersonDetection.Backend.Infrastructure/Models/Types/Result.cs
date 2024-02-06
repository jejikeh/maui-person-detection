namespace PersonDetection.Backend.Application.Models.Types;

public class Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;
    
    protected Result(TValue value)
    {
        _value = value;
        _error = default;
    }

    protected Result(TError? error)
    {
        _value = default;
        _error = error;
    }
    
    public bool IsSuccess => _error is null;
    public bool IsError => _error is not null;
    
    public TValue GetValue() => _value ?? throw new InvalidOperationException("Value is null");
    public TError GetError() => _error ?? throw new InvalidOperationException("Error is null");
    
    public static Result<TValue, TError> Ok(TValue value) => new Result<TValue, TError>(value);
    public static Result<TValue, TError> Error(TError error) => new Result<TValue, TError>(error);
    
    public static implicit operator Result<TValue, TError>(TValue value) => Ok(value);
    public static implicit operator Result<TValue, TError>(TError error) => Error(error);
    public static implicit operator TValue (Result<TValue, TError> result) => result.GetValue()!;
    public static implicit operator TError (Result<TValue, TError> result) => result.GetError()!;
}