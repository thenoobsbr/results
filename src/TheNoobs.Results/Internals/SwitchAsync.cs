using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

public class SwitchAsync<TResult>
{
    private readonly IResult _resultItem;
    private Task<TResult>? _result;

    internal SwitchAsync(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
    }
    
    public Task<TResult> Else(Func<IResult, Task<TResult>> defaultAction)
    {
        return _result ?? defaultAction(_resultItem);
    }

    public SwitchAsync<TResult> Case<TResultItem>(Func<TResultItem, Task<TResult>> action)
        where TResultItem : IResult
    {
        if (_resultItem is not TResultItem item)
        {
            return this;
        }
        
        _result = action(item);
        return this;
    }
}

public class SwitchAsync
{
    private Task? _result;
    private readonly IResult _resultItem;

    internal SwitchAsync(IResult result)
    {
        _resultItem = result ?? throw new ArgumentNullException(nameof(result));
    }
    
    public Task Else(Func<IResult, Task> defaultAction)
    {
        return _result ?? defaultAction(_resultItem);
    }

    public SwitchAsync Case<TResultItem>(Func<TResultItem, Task> action)
        where TResultItem : IResult
    {
        if (_resultItem is not TResultItem item)
        {
            return this;
        }

        _result = action(item);
        return this;
    }
}
