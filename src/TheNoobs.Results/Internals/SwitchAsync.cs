using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

public class SwitchAsync<TResponse>
{
    private readonly IResult _resultItem;
    private bool _executed;
    private Task<TResponse>? _result;

    internal SwitchAsync(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
        _executed = false;
    }

    public SwitchAsync<TResponse> Case<TResult>(Func<TResult, Task<TResponse>> action)
        where TResult : IResult
    {
        if (_executed || _resultItem is not TResult item)
        {
            return this;
        }

        _result = action(item);
        _executed = true;

        return this;
    }

    public Task<TResponse> Else(Func<IResult, Task<TResponse>> defaultAction)
    {
        return _executed
            ? _result!
            : defaultAction(_resultItem);
    }
}

public class SwitchAsync
{
    private readonly IResult _resultItem;
    private bool _executed;
    private Task? _result;

    internal SwitchAsync(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
        _executed = false;
    }

    public SwitchAsync Case<TResult>(Func<TResult, Task> action)
        where TResult : IResult
    {
        if (_executed || _resultItem is not TResult item)
        {
            return this;
        }

        _result = action(item);
        _executed = true;

        return this;
    }

    public Task Else(Func<IResult, Task> defaultAction)
    {
        return _executed
            ? _result!
            : defaultAction(_resultItem);
    }
}
