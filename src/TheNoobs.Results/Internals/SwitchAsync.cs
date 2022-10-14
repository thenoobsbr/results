using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

public class SwitchAsync<TResult>
{
    private readonly IDictionary<Type, Func<Task<TResult>>> _actions;
    private readonly IResult _result;

    internal SwitchAsync(IResult result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _actions = new Dictionary<Type, Func<Task<TResult>>>();
    }

    public SwitchAsync<TResult> Case<TResultItem>(Func<TResultItem, Task<TResult>> action)
        where TResultItem : IResult
    {
        var result = UnWrapper.Unwrap(_result);
        _actions.Add(typeof(TResultItem), () => action((TResultItem) result));
        return this;
    }

    public Task<TResult> Else(Func<IResult, Task<TResult>> defaultAction)
    {
        var result = UnWrapper.Unwrap(_result);
        var action = _actions.FirstOrDefault(a => a.Key.IsInstanceOfType(result)).Value;
        return action is null
            ? defaultAction(result)
            : action();
    }
}

public class SwitchAsync
{
    private readonly IDictionary<Type, Func<Task>> _actions;
    private readonly IResult _result;

    internal SwitchAsync(IResult result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _actions = new Dictionary<Type, Func<Task>>();
    }

    public SwitchAsync Case<TResult>(Func<TResult, Task> action)
        where TResult : IResult
    {
        var result = UnWrapper.Unwrap(_result);
        _actions.Add(typeof(TResult), () => action((TResult) result));
        return this;
    }

    public Task Else(Func<IResult, Task> defaultAction)
    {
        var result = UnWrapper.Unwrap(_result);
        var action = _actions.FirstOrDefault(a => a.Key.IsInstanceOfType(result)).Value;
        return action is null
            ? defaultAction(result)
            : action();
    }
}
