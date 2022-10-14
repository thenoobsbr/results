using TheNoobs.Results.Abstractions;

namespace TheNoobs.Results.Internals;

internal static class UnWrapper
{
    internal static IResult Unwrap(IResult result)
    {
        if (result is IFailWrappedResult wrapped)
        {
            return wrapped.GetFail();
        }

        return result;
    }
}
