using TheNoobs.Results.Internals;

namespace TheNoobs.Results.Extensions;

public static class ValueTaskResultExtensions
{
    public static Switch Switch<T>(this ValueTask<Result<T>> result)
        where T : notnull
    {
        var r = result
            .GetAwaiter()
            .GetResult();

        return r.Switch();
    }

    public static SwitchAsync SwitchAsync<T>(this ValueTask<Result<T>> result)
        where T : notnull
    {
        var r = result
            .GetAwaiter()
            .GetResult();

        return r.SwitchAsync();
    }
}
