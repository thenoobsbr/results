using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

public class Switch<TReturn>
{
    private readonly IResult _resultItem;
    private bool _executed;
    private TReturn? _result;

    internal Switch(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
        _executed = false;
    }

    public Switch<TReturn> Case<TResult>(Func<TResult, TReturn> action)
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

    public TReturn Else(Func<IResult, TReturn> defaultAction)
    {
        return _executed
            ? _result!
            : defaultAction(_resultItem);
    }
}

public class Switch
{
    private readonly IResult _resultItem;
    private bool _executed;

    internal Switch(IResult result)
    {
        _resultItem = UnWrapper.Unwrap(result ?? throw new ArgumentNullException(nameof(result)));
        _executed = false;
    }

    public Switch Case<TResultItem>(Action<TResultItem> action)
        where TResultItem : IResult
    {
        if (_executed || _resultItem is not TResultItem item)
        {
            return this;
        }

        action(item);
        _executed = true;

        return this;
    }

    public void Else(Action<IResult> defaultAction)
    {
        if (_executed)
        {
            return;
        }

        defaultAction(_resultItem);
    }
}
