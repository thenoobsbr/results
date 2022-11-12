using TheNoobs.Results.Internals;

namespace TheNoobs.Results.Extensions;

public static class TaskResultExtensions
{
    public static Switch Switch<T>(this Task<Result<T>> result)
        where T : notnull
    {
        var r = result
            .GetAwaiter()
            .GetResult();

        return r.Switch();
    }

    public static SwitchAsync SwitchAsync<T>(this Task<Result<T>> result)
        where T : notnull
    {
        var r = result
            .GetAwaiter()
            .GetResult();

        return r.SwitchAsync();
    }
}
