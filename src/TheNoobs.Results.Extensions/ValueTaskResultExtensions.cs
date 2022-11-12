using TheNoobs.Results.Internals;

namespace TheNoobs.Results.Extensions;

public static class ValueTaskResultExtensions
{
    public static SwitchAsync Switch<T>(this ValueTask<Result<T>> result)
        where T : notnull
    {
        var r = result
            .GetAwaiter()
            .GetResult();

        return r.SwitchAsync();
    }
}
