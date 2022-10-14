using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

public class Switch<TResult>
{
    private readonly IResult _resultItem;
    private TResult? _result;

    internal Switch(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
    }

    public TResult Else(Func<IResult, TResult> defaultAction)
    {
        if (!Equals(_result, default(TResult)))
        {
            return _result!;
        }
        return defaultAction(_resultItem);
    }

    public Switch<TResult> Case<TResultItem>(Func<TResultItem, TResult> action)
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

public class Switch
{
    private readonly IResult _resultItem;
    private bool _executed;

    internal Switch(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
    }
    
    public void Else(Action<IResult> defaultAction)
    {
        if (_executed)
        {
            return;
        }

        defaultAction(_resultItem);
    }

    public Switch Case<TResultItem>(Action<TResultItem> action)
        where TResultItem : IResult
    {
        if (_resultItem is not TResultItem item)
        {
            return this;
        }

        action(item);
        _executed = true;
        
        return this;
    }
}
