using TheNoobs.Results.Internals;

namespace TheNoobs.Results.Extensions;

public static class TaskResultExtensions
{
    public static SwitchAsync Switch<T>(this Task<Result<T>> result)
        where T : notnull
    {
        var r = result
            .GetAwaiter()
            .GetResult();

        return r.SwitchAsync();
    }
}
