using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

public class Switch<TResult>
{
    private readonly IDictionary<Type, Func<TResult>> _actions;
    private readonly IResult _result;

    internal Switch(IResult result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _actions = new Dictionary<Type, Func<TResult>>();
    }

    public TResult Default(Func<IResult, TResult> defaultAction)
    {
        var result = UnWrapper.Unwrap(_result);
        var action = _actions.FirstOrDefault(a => a.Key.IsInstanceOfType(result)).Value;
        return action is null
            ? defaultAction(result)
            : action();
    }

    public Switch<TResult> Case<TResultItem>(Func<TResultItem, TResult> action)
        where TResultItem : IResult
    {
        var result = UnWrapper.Unwrap(_result);
        _actions.Add(typeof(TResultItem), () => action((TResultItem)result));
        return this;
    }
}

public class Switch
{
    private readonly IDictionary<Type, Action> _actions;
    private readonly IResult _result;

    internal Switch(IResult result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _actions = new Dictionary<Type, Action>();
    }

    public void Default(Action<IResult> defaultAction)
    {
        var result = UnWrapper.Unwrap(_result);
        var action = _actions.FirstOrDefault(a => a.Key.IsInstanceOfType(result)).Value;
        if (action is null)
        {
            defaultAction(result);
            return;
        }

        action();
    }

    public Switch Case<TResult>(Action<TResult> action)
        where TResult : IResult
    {
        var result = UnWrapper.Unwrap(_result);
        _actions.Add(typeof(TResult), () => action((TResult)result));
        return this;
    }
}
