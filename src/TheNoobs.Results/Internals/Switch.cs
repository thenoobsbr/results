using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

public class Switch<TReturn>
{
    private readonly IResult _resultItem;
    private TReturn? _result;

    internal Switch(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
    }

    public TReturn Else(Func<IResult, TReturn> defaultAction)
    {
        if (!Equals(_result, default(TReturn)))
        {
            return _result!;
        }
        return defaultAction(_resultItem);
    }

    public Switch<TReturn> Case<TResultItem>(Func<TResultItem, TReturn> action)
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
